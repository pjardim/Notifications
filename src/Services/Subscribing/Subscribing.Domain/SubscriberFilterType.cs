using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Subscribing.Domain
{

    

    public class SubscriberFilterType : Enumeration
    {
        public static SubscriberFilterType EventPayload = new SubscriberFilterType(1, "EventPayload");
        public static SubscriberFilterType PartyGroup = new SubscriberFilterType(2, "PartyGroup");

        public SubscriberFilterType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<SubscriberFilterType> List() =>
          new[] { EventPayload, PartyGroup };

        public static SubscriberFilterType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new NotifyingDomainException($"Possible values for SubscriberGroup: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static SubscriberFilterType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new NotifyingDomainException($"Possible values for MessageStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}