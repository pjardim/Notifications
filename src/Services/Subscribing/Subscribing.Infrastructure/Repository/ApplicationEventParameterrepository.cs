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
    public class ApplicationEventParameterrepository : RepositoryBase<ApplicationEventParameter>, IApplicationEventParameterRepository
    {
        private readonly SubscriberContext _context;

        public ApplicationEventParameterrepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<IEnumerable<ApplicationEventParameter>> GetByApplicationEventIdAsync(Guid applicationEventId)
        {
            return await _context.ApplicationEventParameters
                .AsNoTracking()
                .Include(x => x.ApplicationEvent)
                .Where(x => x.ApplicationEventId == applicationEventId).ToListAsync();
        }

        public async Task<ApplicationEventParameter> GetByIdWithRelatedAsync(Guid applicationEventParameterId)
        {
            return await _context.ApplicationEventParameters
                .AsNoTracking()
                .Include(x => x.ApplicationEvent)
                .Where(x => x.Id == applicationEventParameterId).FirstOrDefaultAsync();
        }
    }
}