using MediatR;
using Microsoft.Extensions.Logging;
using Notifications.Core.Mediator;
using Notifying.API.Application.DomainEvents;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notifying.API.Application.DomainEventHandlers
{
    public class MessageDomainEventHandler :
           INotificationHandler<MessageStartedDomainEvent>,
           INotificationHandler<NotificationAddedToMessageDomainEvent>

    {
        private readonly IMediatorHandler _mediatorHandler;

        private readonly ILogger<MessageDomainEventHandler> _logger;

        public MessageDomainEventHandler(IMediatorHandler mediatorHandler, ILogger<MessageDomainEventHandler> logger)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task Handle(MessageStartedDomainEvent message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task Handle(NotificationAddedToMessageDomainEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("----- Notification Added To Message - Notification: {@ApplicationEventName}, MessageId : {@MessageId}", notification.ApplicationEventName, notification.MessageId);
        }
    }
}