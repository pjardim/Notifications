using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Events
{
    public class ApplicationEventParameterCreatedEvent : Event
    {
        public ApplicationEventParameterCreatedEvent(Guid id, Guid applicationEventId, string parameterName, string description)
        {
            Id = id;
            ApplicationEventId = applicationEventId;
            ParameterName = parameterName;
            Description = description;
            AggregateId = ApplicationEventId;
        }

        public Guid Id { get; set; }
        public Guid ApplicationEventId { get; set; }
        public string ParameterName { get; set; }
        public string Description { get; set; }
    }

    public class ApplicationEventParameterUpdatedEvent : Event
    {
        public ApplicationEventParameterUpdatedEvent(Guid id, Guid applicationEventId, string parameterName, string description)
        {
            Id = id;
            ApplicationEventId = applicationEventId;
            ParameterName = parameterName;
            Description = description;
            AggregateId = ApplicationEventId;
        }

        public Guid Id { get; set; }
        public Guid ApplicationEventId { get; set; }
        public string ParameterName { get; set; }
        public string Description { get; set; }
    }

    public class ApplicationEventParameterDeletedEvent : Event
    {
        public ApplicationEventParameterDeletedEvent(Guid id, Guid applicationEventId, string parameterName, string description)
        {
            Id = id;
            ApplicationEventId = applicationEventId;
            ParameterName = parameterName;
            Description = description;
            AggregateId = ApplicationEventId;
        }

        public Guid Id { get; set; }
        public Guid ApplicationEventId { get; set; }
        public string ParameterName { get; set; }
        public string Description { get; set; }
    }
}