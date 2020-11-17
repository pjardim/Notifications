using EventBus.Events;
using System;

namespace Notification.BackgroundTasks.Events
{
    public class DelaySendMessageTimeExpiredIntegrationEvent : IntegrationEvent
    {
        public Guid MessageId { get; }

        public DelaySendMessageTimeExpiredIntegrationEvent(Guid messageId) =>
            MessageId = messageId;
    }
}