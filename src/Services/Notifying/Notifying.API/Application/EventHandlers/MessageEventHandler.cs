using MediatR;
using Microsoft.Extensions.Logging;
using Notifying.API.Application.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notifying.API.Application.DomainEventHandlers
{
    public class MessageEventHandler :
           INotificationHandler<ApplicationEventNotFoundEvent>,
           INotificationHandler<PartyIdNotSubscribedToApplicationEvent>,
           INotificationHandler<SubscriberNotFoundEvent>


    {
        private readonly IMediator _mediatorHandler;

        private readonly ILogger<MessageEventHandler> _logger;

        public MessageEventHandler(IMediator mediatorHandler, ILogger<MessageEventHandler> logger)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(ApplicationEventNotFoundEvent notification, CancellationToken cancellationToken)
        {
            //It should save as not Processed in a temp table and requeue when ready.
            _logger.LogWarning("----- ApplicationEventNotFoundDomainEvent - ApplicationEvent: {@ApplicationEvent}", notification.ApplicationEventName);
            throw new System.NotImplementedException();
        }

        public Task Handle(SubscriberNotFoundEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("----- SubscriberNotFoundDomainEvent - Subscriber: {@Subscriber}", notification.RecipientPartyId);
            throw new System.NotImplementedException();
        }

        public Task Handle(PartyIdNotSubscribedToApplicationEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("----- PartyIdNotSubscribedToApplicationDomainEvent - Subscriber: {@Subscriber}", notification.MessageNotification.PublisherPartyId);
            throw new System.NotImplementedException();
        }
    }
}