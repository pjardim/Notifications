using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Events
{
    public class ApplicationEventCreatedEvent : Event
    {
        public ApplicationEventCreatedEvent(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Name { get; }
        public string Description { get; set; }
    }

    public class ApplicationEventUpdatedEvent : Event
    {
        public ApplicationEventUpdatedEvent(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Name { get; }
        public string Description { get; set; }
    }

    public class ApplicationEventDeletedEvent : Event
    {
        public ApplicationEventDeletedEvent(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Name { get; }
        public string Description { get; set; }
    }
}