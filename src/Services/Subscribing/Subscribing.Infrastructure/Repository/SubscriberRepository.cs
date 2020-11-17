using Microsoft.EntityFrameworkCore;
using Notifications.Core.SeedWork;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscribing.Infrastructure.Repository
{
    public class SubscriberRepository : RepositoryBase<Subscriber>, ISubscriberRepository
    {
        private readonly SubscriberContext _context;

        public SubscriberRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<Subscriber>> GetAllSubscribersByGroup(string subscriberGroup)
        {
            return await _context.Subscribers
                .Include(sg => sg.SubscriberGroupSubscriber)
                .ThenInclude(c => c.SubscriberGroup)
                .Where(s => s.SubscriberGroupSubscriber.Any(x => x.SubscriberGroup.Name == subscriberGroup)).ToListAsync();
        }

        public async Task<Subscriber> GetByIdAsync(string recipientPartyId)
        {
            return await _context.Subscribers.Where(x => x.SubscriberPartyId == recipientPartyId).FirstOrDefaultAsync();
        }

        public async Task<Subscriber> GetSubscriberByNameAsync(string subscriberName)
        {
            return await _context.Subscribers.Where(s => s.Name == subscriberName).FirstOrDefaultAsync();
        }

        public async Task<Subscriber> GetWithRelatedAsync(string recipientPartyId)
        {
            return await _context.Subscribers
                .AsNoTracking()
                 .Include(sg => sg.SubscriberGroupSubscriber)
                .ThenInclude(c => c.SubscriberGroup)
                .Where(s => s.SubscriberPartyId == recipientPartyId).FirstOrDefaultAsync();
        }

        public async Task<Subscriber> GetById(string partyId)
        {
            return await _context.Subscribers
                .AsNoTracking()
                .Where(x => x.SubscriberPartyId == partyId).FirstOrDefaultAsync();
        }

        public void Remove(string subscriberPartyId)
        {
            _context.Remove(subscriberPartyId);
        }
    }
}