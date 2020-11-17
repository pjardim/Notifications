using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Notifications.Core.Mediator;
using Notifying.API.Application.Commands;
using Notifying.API.Application.Events;
using Notifying.API.Infrastructure.Repositories;
using Notifying.API.Model;
using Subscribing.Application.Queries;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notifying.API.Application.CommandHandlers
{
    public class MessageCommandHandler :
           IRequestHandler<CreateMessageNotificationCommand, bool>,
           IRequestHandler<SetDelaySendMessageTimeExpiredCommand, bool>,
           IRequestHandler<AddNotificationToMessageCommand, bool>,
           IRequestHandler<CreateNewMessageCommand, bool>,
           IRequestHandler<StartGenericEventProcessCommand, bool>

    {
        private readonly IMediatorHandler _mediatorHandler;

        //private readonly IMessageRepository _messageRepository;

        private readonly INotificationsNoSqlRepository _messageRepository;
        private readonly IApplicationEventQueries _applicationEventQueries;
        private readonly ILogger<MessageCommandHandler> _logger;
        private readonly ISubscriberQueries _subscriberQueries;

        public MessageCommandHandler(IMediatorHandler mediatorHandler,
            INotificationsNoSqlRepository messageRepository,
            IApplicationEventQueries applicationEventQueries,
            ISubscriberQueries subscriberQueries,
            ILogger<MessageCommandHandler> logger)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));
            _applicationEventQueries = applicationEventQueries ?? throw new ArgumentNullException(nameof(applicationEventQueries));
            _subscriberQueries = subscriberQueries ?? throw new ArgumentNullException(nameof(subscriberQueries));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(StartGenericEventProcessCommand command, CancellationToken cancellationToken)
        {
            var applicationEventName = command.ApplicationEvent.Name;
            var payloadDictionary = command.ApplicationEvent.Payload;

            var applicationEvent = await _applicationEventQueries.GetApplicationEventByNameAsync(applicationEventName);
            if (applicationEvent == null)
            { 
                await _mediatorHandler.Publish(new ApplicationEventNotFoundEvent(applicationEventName, command.ApplicationEvent));
                return false;
            }

            var recipientPartyId = "";
            payloadDictionary.TryGetValue("CrewPartyId", out recipientPartyId);

            var recipientParties = await _subscriberQueries.IdentifyRecipientSubscribers(recipientPartyId, applicationEvent.Id);

            if (!recipientParties.Any())
            {
                await _mediatorHandler.Publish(new SubscriberNotFoundEvent(payloadDictionary.Where(x => x.Key == "CrewPartyId").ToString()));
                return false;
            }

            var payload = JsonConvert.SerializeObject(command.ApplicationEvent.Payload);

            //Check if already exist a Message with status pending created to this partyId with the same ApplicationEvent group
            var existingMessageForAnySubscriberPrtyIdAndApplicationGroup = await _messageRepository.GetPendingMessagesByPartyIdAndApplicationEventGroup(recipientParties, applicationEventName);

            if (existingMessageForAnySubscriberPrtyIdAndApplicationGroup != null)
                return await _mediatorHandler.Send(new GroupMessageNotificationCommand(existingMessageForAnySubscriberPrtyIdAndApplicationGroup.Id, applicationEvent.Id, recipientParties, applicationEventName,
                    command.ApplicationEvent.PublisherPartyId, applicationEvent.ApplicationEventChannels.Min(x => x.DelayedSendMinutes), payload));
            else
                return await _mediatorHandler.Send(new CreateNewMessageCommand(Guid.NewGuid(), command.ApplicationEvent.PublisherPartyId, applicationEvent.Id, recipientParties,
                    applicationEvent.ApplicationEventName, applicationEvent.ApplicationEventChannels.Min(x => x.DelayedSendMinutes), payload));
        }

        public async Task<bool> Handle(CreateNewMessageCommand command, CancellationToken cancellationToken)
        {
            var applicationEventChannel = await _applicationEventQueries
                .GetApplicationEventByIdAsync(command.ApplicationEventId);

            var messageNotification = new Notification(Guid.NewGuid(), Guid.NewGuid(), command.PublihserPatyId,
               command.ApplicationEventId, command.ApplicationEventName, command.DelaySendMinutes,
                command.PayLoad);

            messageNotification.AddNotificationSubscribers(command.RecipientsParties);

            //Create Message for Each Channel
            foreach (var channel in applicationEventChannel.ApplicationEventChannels)
            {
                //Create Message with Notificatios
                return await _mediatorHandler.Send(new CreateMessageNotificationCommand(command.PublihserPatyId, channel.Channel.ChannelName, channel.RequireAcknowledgement, messageNotification));
            }

            return true;
        }

        public async Task<bool> Handle(CreateMessageNotificationCommand command, CancellationToken cancellationToken)
        {
            var newMessage = new Message(Guid.NewGuid(), command.Channel, command.RequireAcknowledgement);
            var messageNotification = command.MessageNotification;

            var newMessageNotification = new Notification(Guid.NewGuid(), newMessage.Id,
                messageNotification.PublisherPartyId, messageNotification.ApplicationEventId,
                messageNotification.ApplicationEventName, messageNotification.DelaySendMinutes, messageNotification.PayLoad);

            newMessageNotification.AddNotificationSubscribers(messageNotification.NotificationSubscribersPartyIds.ToList());

            newMessage.AddMessageNotification(newMessageNotification);

            await _messageRepository.AddMessage(newMessage);

            return true;
        }

        public async Task<bool> Handle(AddNotificationToMessageCommand command, CancellationToken cancellationToken)
        {
            var existingMessage = await _messageRepository.GetMesssageAsync(command.MessageId);

            var messageNotification = command.MessageNotification;

            var newMessageNotification = new Notification(messageNotification.Id, existingMessage.Id, messageNotification.PublisherPartyId,
               messageNotification.ApplicationEventId, messageNotification.ApplicationEventName, messageNotification.DelaySendMinutes,
                messageNotification.PayLoad);

            newMessageNotification.AddNotificationSubscribers(messageNotification.NotificationSubscribersPartyIds.ToList());

            existingMessage.AddMessageNotification(newMessageNotification);

            await _messageRepository.UpdateMessage(existingMessage);
            return true;
        }

        public async Task<bool> Handle(SetDelaySendMessageTimeExpiredCommand command, CancellationToken cancellationToken)
        {
            var messageToUpdate = await _messageRepository.GetMesssageAsync(command.MessageId);
            if (messageToUpdate == null)
            {
                return false;
            }

            messageToUpdate.SetDelayMessageTimeExpiredStatus();

            await _messageRepository.UpdateMessage(messageToUpdate);

            return true;
        }
    }
}