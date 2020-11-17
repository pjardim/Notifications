using Notifications.Core.SeedWork;

namespace Subscribing.Domain
{
    public class SubscriberGroupSubscriber : Entity, IAggregateRoot
    {
        public SubscriberGroupSubscriber(string subscriberPartyId, int subscriberGroupId)
        {
            SubscriberPartyId = subscriberPartyId;
            SubscriberGroupId = subscriberGroupId;
        }

        public string SubscriberPartyId { get; private set; }
        public Subscriber Subscriber { get; private set; }

        public int SubscriberGroupId { get; private set; }
        public SubscriberGroup SubscriberGroup { get; private set; }
    }
}