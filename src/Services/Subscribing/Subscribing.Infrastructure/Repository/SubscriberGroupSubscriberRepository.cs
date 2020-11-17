using Microsoft.EntityFrameworkCore;
using Notifications.Core.SeedWork;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscribing.Infrastructure.Repository
{
    public class SubscriberGroupSubscriberRepository : RepositoryBase<SubscriberGroupSubscriber>, ISubscriberGroupSubscriberRepository
    {
        private readonly SubscriberContext _context;

        public SubscriberGroupSubscriberRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<SubscriberGroupSubscriber>> GetAllByPartyIdAsync(string partyId)
        {
            return await _context.SubscriberGroupSubscribers
               .Where(s => s.SubscriberPartyId == partyId)
               .Include(x => x.SubscriberGroup)
               .AsNoTracking()
               .ToListAsync();
        }

        public override void Remove(SubscriberGroupSubscriber subscriberGroupSubscriber)
        {
            _context.Entry(subscriberGroupSubscriber).State = EntityState.Deleted;
            _context.Remove(subscriberGroupSubscriber);
        }
    }
}