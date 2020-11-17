using Notifications.Core.Messaging;

namespace Subscribing.Application.Events
{
    public class SubscriberCreatedEvent : Event
    {
        public SubscriberCreatedEvent(string subscriberPartyId, string email, string name)
        {
            SubscriberPartyId = subscriberPartyId;
            Email = email;
            Name = name;
        }

        public string SubscriberPartyId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
    }

    public class SubscriberUpdatedEvent : Event
    {
        public SubscriberUpdatedEvent(string subscriberPartyId, string email, string name)
        {
            SubscriberPartyId = subscriberPartyId;
            Email = email;
            Name = name;
        }

        public string SubscriberPartyId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
    }

    public class SubscriberDeletedEvent : Event
    {
        public SubscriberDeletedEvent(string subscriberPartyId, string email, string name)
        {
            SubscriberPartyId = subscriberPartyId;
            Email = email;
            Name = name;
        }

        public string SubscriberPartyId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }
    }
}