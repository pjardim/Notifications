using MediatR;
using Microsoft.Extensions.Logging;
using Subscribing.Application.IntegrationEvents;
using Subscribing.Application.IntegrationEvents.Events;
using Subscribing.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscribing.Application.DomainEventHandlers.MailRecievedEvent
{
    public class SendNotificationWhenRecieveNewEmailDomainEventHandler : INotificationHandler<EmailRecievedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly ISubscribingIntegrationEventService _subscribingIntegrationEventService;

        public SendNotificationWhenRecieveNewEmailDomainEventHandler(ILoggerFactory logger, ISubscribingIntegrationEventService subscribingIntegrationEventService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
            _subscribingIntegrationEventService = subscribingIntegrationEventService ?? throw new ArgumentNullException(nameof(subscribingIntegrationEventService));
        }

        public async Task Handle(EmailRecievedDomainEvent emailRecievedDomainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<EmailRecievedDomainEvent>()
                .LogTrace("Email Recieved  Id: {messageId} from: {SenderPartyIds}",
                    emailRecievedDomainEvent.MailBoxItem.Id);

            var newMessageCreatedntegrationEvent = new MailCreatedIntegrationEvent(emailRecievedDomainEvent.MessageId,
                emailRecievedDomainEvent.RecipientPartyId, emailRecievedDomainEvent.SenderPartyIds, emailRecievedDomainEvent.Subject, emailRecievedDomainEvent.Body, emailRecievedDomainEvent.RequireAcknowledged);
            await _subscribingIntegrationEventService.AddAndSaveEventAsync(newMessageCreatedntegrationEvent);
        }
    }
}