using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Events
{
    public class SubscriberApplicationEventCreatedEvent : Event
    {
        public SubscriberApplicationEventCreatedEvent(string subscriberPartyId, Guid applicationEventId, Guid channelId)
        {
            SubscriberPartyId = subscriberPartyId;
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            AggregateId = applicationEventId;
        }

        public string SubscriberPartyId { get; set; }
        public Guid ApplicationEventId { get; set; }
        public Guid ChannelId { get; set; }
    }

    public class SubscriberApplicationEventUpdatedEvent : Event
    {
        public SubscriberApplicationEventUpdatedEvent(string subscriberPartyId, Guid applicationEventId, Guid channelId)
        {
            SubscriberPartyId = subscriberPartyId;
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            AggregateId = applicationEventId;
        }

        public string SubscriberPartyId { get; set; }
        public Guid ApplicationEventId { get; set; }
        public Guid ChannelId { get; set; }
    }
}