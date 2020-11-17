using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Subscribing.Domain
{
    public class SubscriberGroup : Enumeration, IAggregateRoot
    {
        public static SubscriberGroup CrewMember = new SubscriberGroup(1, "CrewMember");
        public static SubscriberGroup SystemAdmin = new SubscriberGroup(2, "SystemAdmin");
        public static SubscriberGroup BackOffice = new SubscriberGroup(3, "BackOffice");

        public SubscriberGroup(int id, string name)
            : base(id, name)
        {

        }

        public static IEnumerable<SubscriberGroup> List() =>
          new[] { SystemAdmin, CrewMember, BackOffice };

        public static SubscriberGroup FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new NotifyingDomainException($"Possible values for SubscriberGroup: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static SubscriberGroup From(int id)
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