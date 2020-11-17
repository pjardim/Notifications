using Notifications.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Core.Data.EventSourcing
{
    public interface IEventSourcingRepository
    {
        Task SaveEvent<TEvent>(TEvent @event) where TEvent : Event;

        Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId);
        Task<IEnumerable<StoredEvent>> GetAllEvents();
    }
}