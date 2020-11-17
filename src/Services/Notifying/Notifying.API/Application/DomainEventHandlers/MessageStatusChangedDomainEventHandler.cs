 using MediatR;
using Microsoft.Extensions.Logging;
using Notifying.API.Application.DomainEvents;
using Notifying.API.Application.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notifying.API.Application.DomainEventHandlers
{
    public class MessageStatusChangedDomainEventHandler : INotificationHandler<MessageStatusChangedToDelayTimeExpiredDomainEvent>
    {
        private readonly ILoggerFactory _logger;

        public MessageStatusChangedDomainEventHandler(ILoggerFactory logger)
        {
            _logger = logger;
        }

        public MessageStatusChangedDomainEventHandler(Guid messageId)
        {
            MessageId = messageId;
        }

        public Guid MessageId { get; private set; }

        public async Task Handle(MessageStatusChangedToDelayTimeExpiredDomainEvent messageStatusChangedToDelayTimeExpiredEvent, CancellationToken cancellationToken)
        {
            
        }
    }
}