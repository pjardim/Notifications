using EventBus.Events;
using System;

namespace Notifying.API.Application.IntegrationEvents.Events
{
    public class DelaySendMessageTimeExpiredIntegrationEvent : IntegrationEvent
    {
        public Guid MessageId { get; }
        

        public DelaySendMessageTimeExpiredIntegrationEvent(Guid messageId) =>
            MessageId = messageId;
    }
}