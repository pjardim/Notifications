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
    public class ApplicationEventRepository : RepositoryBase<ApplicationEvent>, IApplicationEventRepository
    {
        private readonly SubscriberContext _context;

        public ApplicationEventRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<ApplicationEvent> GetApplicationEventWithRelatedAsync(Guid applicationEventId)
        {
            return await _context.ApplicationEvents
                .Include(a => a.SubscriberFilters)
                .Include(a => a.SubscriberApplicationEvents)
                .Include(a => a.ApplicationEventParameters)
                .Include(a => a.ApplicationEventChannels)
                .ThenInclude(a => a.Channel)
                .FirstAsync(a => a.Id == applicationEventId);
        }

        public async Task<ApplicationEvent> Get(Guid applicationEventId)
        {
            return await _context.ApplicationEvents
                .Include(a => a.SubscriberFilters)
                .Include(a => a.SubscriberApplicationEvents)
                .Include(a => a.ApplicationEventParameters)
                .Include(a => a.ApplicationEventChannels)
                .FirstAsync(a => a.Id == applicationEventId);
        }

        public async Task<IEnumerable<SubscriberFilter>> GetAllSubscriberFiltersAsync()
        {
            return await _context.SubscriberFilters.ToListAsync();
        }

        public async Task<ApplicationEvent> GetApplicationEventByNameAsync(string applicationEventName)
        {
            return await _context.ApplicationEvents
                  .Include(a => a.SubscriberFilters)
                  .Include(a => a.SubscriberApplicationEvents)
                  .Include(a => a.ApplicationEventParameters)
                  .Include(a => a.ApplicationEventChannels)
                  .ThenInclude(a => a.Channel)
                .Where(x => x.ApplicationEventName == applicationEventName).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ApplicationEvent>> GetAllApplicationEventsWithRelatedAsync()
        {
            return await _context.ApplicationEvents
               .Include(a => a.SubscriberFilters)
               .Include(a => a.SubscriberApplicationEvents)
               .Include(a => a.ApplicationEventParameters)
               .Include(a => a.ApplicationEventChannels)
               .ThenInclude(a => a.Channel)
              .ToListAsync();
        }
    }
}