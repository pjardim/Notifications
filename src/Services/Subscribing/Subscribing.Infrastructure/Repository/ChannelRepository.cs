using Notifications.Core.SeedWork;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;

namespace Subscribing.Infrastructure.Repository
{
    public class ChannelRepository : RepositoryBase<Channel>, IChannelRepository
    {
        private readonly SubscriberContext _context;

        public ChannelRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;
    }
}