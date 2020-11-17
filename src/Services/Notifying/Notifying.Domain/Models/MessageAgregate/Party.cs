using Notifications.Core.SeedWork;
using System.Collections.Generic;

namespace Notifying.Domain.Models.MessageAgregate
{
    public class Party : Entity, IAggregateRoot
    {
        public Party()
        {
            Notifications = new List<Notification>();
            NotificationSubscribers = new List<NotificationSubscriber>();
        }

        public Party(string partyId, string name, string email, string subscriberGroup) : this()
        {
            PartyId = partyId;
            Name = name;
            Email = email;
            SubscriberGroup = subscriberGroup;
        }

        public string PartyId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string SubscriberGroup { get; private set; }

        public ICollection<Notification> Notifications { get; private set; }

        public ICollection<NotificationSubscriber> NotificationSubscribers { get; private set; }
    }
}