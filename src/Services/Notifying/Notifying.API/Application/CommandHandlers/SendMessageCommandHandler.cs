using EventBus.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notifications.Core.Mediator;
using Notifying.API.Application.Commands;
using Notifying.API.Application.IntegrationEvents.Events;
using Notifying.API.Infrastructure.Repositories;
using Notifying.API.Model;
using Subscribing.Application.Queries;
using Subscribing.Application.Queries.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notifying.API.Application.CommandHandlers
{
    public class SendMessageCommandHandler :
           IRequestHandler<SendEmailMessageCommand, bool>,
           IRequestHandler<SendSMSMessageCommand, bool>,
           IRequestHandler<SendNotificationsAppMessageCommand, bool>,
           IRequestHandler<StartSendingMessageCommand, bool>

    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly INotificationsNoSqlRepository _messageRepository;
        private readonly IEventBus _eventBus;
        private readonly IConfiguration _configuration;
        private readonly IApplicationEventQueries _applicationEventQueries;
        private readonly ILogger<MessageCommandHandler> _logger;

        public SendMessageCommandHandler(IMediatorHandler mediatorHandler,
             INotificationsNoSqlRepository messageRepository,
             IConfiguration configuration,
             IEventBus eventBus,
             IApplicationEventQueries applicationEventQueries,
             ISubscriberQueries subscriberQueries,
             ILogger<MessageCommandHandler> logger)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _applicationEventQueries = applicationEventQueries ?? throw new ArgumentNullException(nameof(applicationEventQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(StartSendingMessageCommand command, CancellationToken cancellationToken)
        {
            var message = await _messageRepository.GetMesssageAsync(command.MessageId);

            if (message == null) return false;

            //Todo: Create Template Table inside notifying API and use the event on  subscrbing API to also update this table
            // this way we can run the notifyng API  even the subscriber is down. using CQRS and Integration Events  (also this notifying api could use NoSql for better performance)
            //Note: Every Micro Service should be independent and dont share  models. ( Notifications Template is being shared here)
            var template = await _applicationEventQueries.GetNotificationTemplateData(
                message.Notifications.FirstOrDefault().ApplicationEventId, message.MessageChannel);

            if (template == null)
            {
                //No Template found
            }

            //All messages will be saved by default in the Notifications App
            //they also can be send to anothers channels.
            await _mediatorHandler.Send(new SendNotificationsAppMessageCommand(message, template));

            return message.MessageChannel switch
            {
                //Todo Make it an Ennun
                "Email" => await _mediatorHandler.Send(new SendEmailMessageCommand(message)),
                "SMS" => await _mediatorHandler.Send(new SendSMSMessageCommand(message)),
                _ => false,//Channel not found
            };
        }

        public async Task<bool> Handle(SendEmailMessageCommand command, CancellationToken cancellationToken)
        {
            return false;
        }

        public async Task<bool> Handle(SendSMSMessageCommand command, CancellationToken cancellationToken)
        {
            return false;
        }

        public async Task<bool> Handle(SendNotificationsAppMessageCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var template = command.Template;

                var messageNotifications = command.Message.Notifications;
                var subscribersPartyId = new List<string>();

                //Settings could come from DB
                var defaultParameters = new Dictionary<string, string>
                {
                    { "CrewPayReportLink" , _configuration["CrewPayReportLink"]},
                    { "CrewPayReportDescription" , _configuration["CrewPayReportDescription"]}
                };

                AssembleNotificationsTable(template, messageNotifications, defaultParameters);

                subscribersPartyId.AddRange(messageNotifications.SelectMany(x => x.NotificationSubscribersPartyIds));

                ReplaceBodyTagsWithParameterValue(template, messageNotifications.FirstOrDefault().PayLoad);

                if (command.Message.RequireAcknowledgement)
                    template.Body += RequireAcknowledgementSection(template.Body, command.Message.Id);

                foreach (var recipientPartyId in subscribersPartyId.Distinct())
                {
                    var messagePublishers = string.Join(",", messageNotifications.Select(x => x.PublisherPartyId).Distinct());

                    var messageSentEvent = new MessageSentIntegrationEvent(command.MessageId,
                        messagePublishers, recipientPartyId, template.Subject, template.Body, command.Message.RequireAcknowledgement);
                    _eventBus.Publish(messageSentEvent);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("--- Error on SendNotificationsAppMessageCommand", ex);
                return false;
            }
        }

        private string RequireAcknowledgementSection(string body, Guid messageId)
        {
            var requireAcknowledgementBuilder = new StringBuilder();

            var link = "<a href = '" + _configuration["SubscribingRequireAcknowledgementLink"] + "messageId=" + messageId + "'>This Message require Acknowledgement. Please click here to acknowledge it</a>";

            requireAcknowledgementBuilder.Append("<br>");
            requireAcknowledgementBuilder.Append(link);
            requireAcknowledgementBuilder.Append("<br>");

            return requireAcknowledgementBuilder.ToString();


        }

        public string ReplaceBodyTagsWithParameterValue(ApplicationEventChannelTemplateViewModel template, string parameters)
        {
            var jObj = (JObject)JsonConvert.DeserializeObject(parameters);

            foreach (var item in jObj)
            {
                var paramter = "{{" + item.Key + "}}";

                if (template.Body.Contains(paramter))
                    template.Body = template.Body.Replace(paramter, Convert.ToString(item.Value));
            }

            return template.Body;
        }

        public void AssembleNotificationsTable(ApplicationEventChannelTemplateViewModel template, IEnumerable<Notification> messageNotifications, Dictionary<string, string> defaultParameters)
        {
            var htmlTable = new StringBuilder();

            messageNotifications = messageNotifications.OrderByDescending(x => x.CreatedDate);

            htmlTable.Append("<table id='notificationsTable' class='table table - striped'>");
            htmlTable.Append("<tr>");

            foreach (var parameter in template.ApplicationEvent.ApplicationEventParameters)
            {
                htmlTable.Append("<th>");
                htmlTable.Append(parameter.ParameterName);
                htmlTable.Append("</th>");
            }
            htmlTable.Append("<th>");
            htmlTable.Append("Date");
            htmlTable.Append("</th>");
            htmlTable.Append("</tr>");

            foreach (var notification in messageNotifications)
            {
                var jObject = (JObject)JsonConvert.DeserializeObject(notification.PayLoad);
                AddTableItem(template, jObject, htmlTable, notification.CreatedDate, defaultParameters);
            }

            htmlTable.Append("</table>");

            template.Body = template.Body.Replace("[[Notifications]]", htmlTable.ToString());
        }

        private void AddTableItem(ApplicationEventChannelTemplateViewModel template, JObject jObject, StringBuilder htmlTable, DateTime createdDate, Dictionary<string, string> defaultParameters)
        {
            htmlTable.Append("<tr>");
            foreach (var parameter in template.ApplicationEvent.ApplicationEventParameters)
            {
                var formatedParamter = GetFormatedParameter(jObject, parameter, defaultParameters);
                htmlTable.Append("<td>");
                if (jObject[parameter.ParameterName] != null)
                    htmlTable.Append(formatedParamter);
                htmlTable.Append("</td>");
            }
            htmlTable.Append("<td>");
            htmlTable.Append(createdDate.ToString());
            htmlTable.Append("</td>");

            htmlTable.Append("</tr>");
        }

        private string GetFormatedParameter(JObject jObject, ApplicationEventParameterViewModel parameter, Dictionary<string, string> defaultParameters)
        {
            var crewPayReportLink = defaultParameters.FirstOrDefault(t => t.Key == "CrewPayReportLink").Value;
            var CrewPayReportDescription = defaultParameters.FirstOrDefault(t => t.Key == "CrewPayReportDescription").Value;

            switch (parameter.ParameterName)
            {
                case "CrewPayReportDetails":
                    return $"<a href=\"{crewPayReportLink}?{jObject[parameter.ParameterName]}\">{CrewPayReportDescription}</a></p>";

                default:
                    return jObject[parameter.ParameterName].ToString();
            }
        }
    }
}