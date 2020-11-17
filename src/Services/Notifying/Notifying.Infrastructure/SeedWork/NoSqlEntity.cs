using Notifications.Core.Messaging;
using System.Collections.Generic;

namespace Notifying.Infrastructure.SeedWork
{
    public abstract class NoSqlEntity
    {
        private List<Event> _domainEvents;
        public IReadOnlyCollection<Event> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(Event eventItem)
        {
            _domainEvents = _domainEvents ?? new List<Event>();
            _domainEvents.Add(eventItem);
        }

        public void RemoveDomainEvent(Event eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }
    }
}