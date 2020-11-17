using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Events
{
    internal class NotificationTemplateCreatedEvent : Event
    {
        public NotificationTemplateCreatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Name { get; }
    }

    public class NotificationTemplateUpdatedEvent : Event
    {
        public NotificationTemplateUpdatedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Name { get; }
    }

    public class NotificationTemplateDeletedEvent : Event
    {
        public NotificationTemplateDeletedEvent(Guid id, string name)
        {
            Id = id;
            Name = name;
            AggregateId = id;
        }

        public Guid Id { get; set; }
        public string Name { get; }
    }
}