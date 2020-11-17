using Notifying.Core.Data.EventSourcing;
using Notifying.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventSourcing
{
    public class EventSourcingRepository : IEventSourcingRepository
    {
        private readonly IEventStoreService _eventStoreService;

        public EventSourcingRepository(IEventStoreService eventStoreService)
        {
            _eventStoreService = eventStoreService;
        }

        public async Task SaveEvent<TEvent>(TEvent @event) where TEvent : Event
        {
        }

        public async Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId)
        {
            return new List<StoredEvent>();
        }
    }

    internal class BaseEvent
    {
        public DateTime Timestamp { get; set; }
    }
}