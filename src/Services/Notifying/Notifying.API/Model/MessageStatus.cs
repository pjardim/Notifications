using Notifications.Core.SeedWork;
using Notifying.API.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Notifying.API.Model
{
    public class MessageStatus : Enumeration
    {
        public static MessageStatus Pending = new MessageStatus(1, "Pending");
        public static MessageStatus WaitingDeleyPeriodTime = new MessageStatus(2, "WaitingDeleyPeriodTime");
        public static MessageStatus DelayTimeExpired = new MessageStatus(3, "DelayTimeExpired");
        public static MessageStatus Sent = new MessageStatus(4, "Sent");
        public static MessageStatus Canceled = new MessageStatus(5, "Canceled");

        public MessageStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<MessageStatus> List() =>
          new[] { Pending, WaitingDeleyPeriodTime, DelayTimeExpired, Sent, Canceled };

        public static MessageStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new NotifyingDomainException($"Possible values for MessageStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static MessageStatus From(int id)
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