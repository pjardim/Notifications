using Notifications.Core.SeedWork;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface ISubscriberGroupSubscriberRepository : IRepository<SubscriberGroupSubscriber>, IRepositoryBase<SubscriberGroupSubscriber>
    {
        Task<IEnumerable<SubscriberGroupSubscriber>> GetAllByPartyIdAsync(string partyId);
        void Remove(SubscriberGroupSubscriber subscriber);
    }
}