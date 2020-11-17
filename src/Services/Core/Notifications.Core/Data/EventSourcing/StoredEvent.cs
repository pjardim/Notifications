using System;

namespace Notifications.Core.Data.EventSourcing
{
    public class StoredEvent
    {
        public StoredEvent(Guid id, string type, DateTime eventDate, string data)
        {
            Id = id;
            Type = type;
            EventDate = eventDate;
            Data = data;
        }

        public Guid Id { get; private set; }

        public string Type { get; private set; }

        public DateTime EventDate { get; set; }

        public string Data { get; private set; }
    }
}