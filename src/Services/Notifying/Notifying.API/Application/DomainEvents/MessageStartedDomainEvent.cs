using Notifications.Core.Messaging;
using Notifications.Core.Messaging.CommonMessages.DomainEvents;
using System;

namespace Notifying.API.Application.DomainEvents
{
    public class MessageStartedDomainEvent : DomainEvent
    {
        public MessageStartedDomainEvent(Guid messageId, string channel) : base(messageId)
        {
            MessageId = messageId;
            Channel = channel;

        }

        public Guid MessageId { get; private set; }

        public string Channel { get; private set; }
    }
}