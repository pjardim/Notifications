using Microsoft.EntityFrameworkCore;
using Notifications.Core.SeedWork;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Subscribing.Infrastructure.Repository
{
    public class ApplicationEventChannelTemplateRepository : RepositoryBase<ApplicationEventChannelTemplate>, IApplicationEventChannelTemplateRepository
    {
        private readonly SubscriberContext _context;

        public ApplicationEventChannelTemplateRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task<ApplicationEventChannelTemplate> GetByApplicationEventAndChannel(Guid applicationEventId, Guid channelId)
        {
            return await _context.ApplicationEventChannelTemplates
                .AsNoTracking()
                .Include(x => x.ApplicationEvent)
                .ThenInclude(x => x.ApplicationEventParameters)
                .Where(x => x.ApplicationEventId == applicationEventId && x.ChannelId == channelId).FirstOrDefaultAsync();
        }

        public async Task<ApplicationEventChannelTemplate> GetByApplicationEventAndChannel(Guid applicationEventId, string messageChannel)
        {
            return await _context.ApplicationEventChannelTemplates
                .AsNoTracking()
                .Include(x => x.ApplicationEvent)
                .ThenInclude(x => x.ApplicationEventParameters)
                .Where(x => x.ApplicationEventId == applicationEventId && x.Channel.ChannelName == messageChannel).FirstOrDefaultAsync();
        }
    }
}