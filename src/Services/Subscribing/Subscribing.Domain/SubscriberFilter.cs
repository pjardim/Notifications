using Notifications.Core.SeedWork;
using System;

namespace Subscribing.Domain
{
    public class SubscriberFilter : Entity, IAggregateRoot
    {
        public Guid ApplicationEventId { get; private set; }
        public ApplicationEvent ApplicationEvent { get; private set; }
        public string FilterType { get; private set; }
        public string FilterValue { get; private set; }



        protected SubscriberFilter()
        {
        }

        public SubscriberFilter(Guid id, Guid applicationEventId, string filterType, string filterValue) : this()
        {
            Id = id;
            ApplicationEventId = applicationEventId;
            FilterType = filterType;
            FilterValue = filterValue;
        }
    }
}