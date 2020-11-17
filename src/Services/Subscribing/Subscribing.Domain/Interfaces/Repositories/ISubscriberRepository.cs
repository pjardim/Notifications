using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface ISubscriberRepository : IRepository<Subscriber>, IRepositoryBase<Subscriber>
    {
        Task<Subscriber> GetById(string partyId);
        Task<Subscriber> GetSubscriberByNameAsync(string subscriberName);
        Task<IEnumerable<Subscriber>> GetAllSubscribersByGroup(string subscriberGroup);
        Task<Subscriber> GetWithRelatedAsync(string recipientPartyId);
        Task<Subscriber> GetByIdAsync(string recipientPartyId);
        void Remove(string subscriberPartyId);
    }
}