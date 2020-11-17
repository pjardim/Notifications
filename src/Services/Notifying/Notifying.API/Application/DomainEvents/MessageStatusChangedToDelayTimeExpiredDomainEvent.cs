using Notifications.Core.Messaging.CommonMessages.DomainEvents;
using Notifying.API.Model;
using System;
using System.Collections.Generic;

namespace Notifying.API.Application.DomainEvents
{
    public class MessageStatusChangedToDelayTimeExpiredDomainEvent : DomainEvent
    {
        public Guid MessageId { get; private set; }
        public IEnumerable<Notification> Notifications { get; private set; }

        public MessageStatusChangedToDelayTimeExpiredDomainEvent(Guid messageId) : base(messageId)
        {
            MessageId = messageId;
        }
    }
}