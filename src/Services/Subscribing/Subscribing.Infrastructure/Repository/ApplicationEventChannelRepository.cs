using Microsoft.EntityFrameworkCore;
using Notifications.Core.SeedWork;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Subscribing.Infrastructure.Repository
{
    public class ApplicationEventChannelRepository : RepositoryBase<ApplicationEventChannel>, IApplicationEventChannelRepository
    {
        private readonly SubscriberContext _context;

        public ApplicationEventChannelRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<ApplicationEventChannel> GetByApplicationEventIdAndChannelAsync(Guid applicationEventId, Guid channelId)
        {
            return await _context.ApplicationEventChannels
                .AsNoTracking()
                .Include(x => x.ApplicationEvent)
                .Include(x => x.Channel)
                .Where(x => x.ApplicationEventId == applicationEventId && x.ChannelId == channelId).FirstOrDefaultAsync();
        }
    }
}