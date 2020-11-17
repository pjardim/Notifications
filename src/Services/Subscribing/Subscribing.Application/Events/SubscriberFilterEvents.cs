using Notifications.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscribing.Application.Events
{
    class SubscriberFilterCreatedEvent : Event
    {
        public SubscriberFilterCreatedEvent(Guid id, Guid applicationEventId, string filterType, string filterValue)
        {
            Id = id;
            ApplicationEventId = applicationEventId;
            FilterType = filterType;
            FilterValue = filterValue;
            AggregateId = applicationEventId;
        }

        public Guid Id { get; }
        public Guid ApplicationEventId { get; private set; }
        public string FilterType { get; private set; }
        public string FilterValue { get; private set; }
    }
}
