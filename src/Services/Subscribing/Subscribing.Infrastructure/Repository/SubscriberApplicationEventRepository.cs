using Microsoft.EntityFrameworkCore;
using Notifications.Core.SeedWork;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscribing.Infrastructure.Repository
{
    public class SubscriberApplicationEventRepository : RepositoryBase<SubscriberApplicationEvent>, ISubscriberApplicationEventRepository
    {
        private readonly SubscriberContext _context;

        public SubscriberApplicationEventRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<SubscriberApplicationEvent> GetSubscribersApplicationEvent(string partyId, Guid applicationEventId)
        {
            return await _context.SubscriberApplicationEvents
                .Where(s => s.SubscriberPartyId == partyId && s.ApplicationEventId == applicationEventId)
                .Include(x => x.ApplicationEvent)
                .Include(x => x.Channel)
                .Include(x => x.Subscriber)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SubscriberApplicationEvent>> GetSubscribersApplicationEventsByPartyId(string partyId)
        {
            return await _context.SubscriberApplicationEvents
                 .Where(s => s.SubscriberPartyId == partyId)
                 .Include(x => x.ApplicationEvent)
                 .Include(x => x.Channel)
                 .Include(x => x.Subscriber)
                 .AsNoTracking()
                 .ToListAsync();
        }
    }
}