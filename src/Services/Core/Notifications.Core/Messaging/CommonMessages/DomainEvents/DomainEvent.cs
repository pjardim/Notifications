using MediatR;
using System;

namespace Notifications.Core.Messaging.CommonMessages.DomainEvents
{
    public abstract class DomainEvent : DomainMessage, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected DomainEvent(Guid aggregateId)
        {
            AggregateId = aggregateId;
            Timestamp = DateTime.Now;
        }
    }
}