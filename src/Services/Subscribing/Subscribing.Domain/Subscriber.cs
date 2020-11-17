using Notifications.Core.SeedWork;
using System.Collections.Generic;

namespace Subscribing.Domain
{
    public class Subscriber : Entity, IAggregateRoot
    {
        protected Subscriber()
        {
            SubscriberApplicationEvents = new List<SubscriberApplicationEvent>();
            _subscriberGroupSubscriber = new List<SubscriberGroupSubscriber>();
            RecipientMailBoxItems = new List<MailBoxItem>();
        }

        public Subscriber(string subscriberPartyId, string email, string name) : this()
        {
            SubscriberPartyId = subscriberPartyId;
            Email = email;
            Name = name;
        }

        public string SubscriberPartyId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }

        private readonly List<SubscriberGroupSubscriber> _subscriberGroupSubscriber;
        public IReadOnlyCollection<SubscriberGroupSubscriber> SubscriberGroupSubscriber => _subscriberGroupSubscriber;

        public ICollection<SubscriberApplicationEvent> SubscriberApplicationEvents { get; private set; }

        public ICollection<MailBoxItem> RecipientMailBoxItems { get; set; }
    }
}