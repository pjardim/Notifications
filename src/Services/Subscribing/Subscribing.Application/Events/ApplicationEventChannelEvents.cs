using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Events
{
    public class ApplicationEventChannelUpdatedEvent : Event
    {
        public ApplicationEventChannelUpdatedEvent(Guid applicationEventId, Guid channelId, int delayedSendMinutes, bool enabled)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            DelayedSendMinutes = delayedSendMinutes;
            Enabled = enabled;
            AggregateId = applicationEventId;
        }

        public Guid ApplicationEventId { get; set; }
        public Guid ChannelId { get; set; }

        public int DelayedSendMinutes { get; set; }
        public bool Enabled { get; set; }
    }

    public class ApplicationEventChannelCreatedEvent : Event
    {
        public ApplicationEventChannelCreatedEvent(Guid applicationEventId, Guid channelId, int delayedSendMinutes, bool enabled)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            DelayedSendMinutes = delayedSendMinutes;
            Enabled = enabled;
            AggregateId = applicationEventId;
        }

        public Guid ApplicationEventId { get; set; }
        public Guid ChannelId { get; set; }

        public int DelayedSendMinutes { get; set; }
        public bool Enabled { get; set; }
    }
}