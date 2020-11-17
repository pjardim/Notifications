using Notifications.Core.SeedWork;
using System;

namespace Subscribing.Domain
{
    public class SubscriberApplicationEvent : Entity, IAggregateRoot
    {
        protected SubscriberApplicationEvent()
        {
        }

        public SubscriberApplicationEvent(string subscriberPartyId, Guid applicationEventId, Guid channelId) : this()
        {
            SubscriberPartyId = subscriberPartyId;
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
        }

        public string SubscriberPartyId { get; private set; }
        public Subscriber Subscriber { get; private set; }
        public Guid ApplicationEventId { get; private set; }
        public ApplicationEvent ApplicationEvent { get; private set; }

        public Guid ChannelId { get; set; }
        public Channel Channel { get; set; }
    }
}