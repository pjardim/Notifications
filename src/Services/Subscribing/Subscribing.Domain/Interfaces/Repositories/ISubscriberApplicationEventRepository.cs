using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface ISubscriberApplicationEventRepository : IRepository<SubscriberApplicationEvent>, IRepositoryBase<SubscriberApplicationEvent>
    {
        Task<IEnumerable<SubscriberApplicationEvent>> GetSubscribersApplicationEventsByPartyId(string partyId);

        Task<SubscriberApplicationEvent> GetSubscribersApplicationEvent(string partyId, Guid applicationEventId);
    }
}