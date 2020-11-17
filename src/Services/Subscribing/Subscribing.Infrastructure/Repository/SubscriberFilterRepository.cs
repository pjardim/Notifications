using Notifications.Core.SeedWork;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System.Collections.Generic;

namespace Subscribing.Infrastructure.Repository
{
    public class SubscriberFilterRepository : RepositoryBase<SubscriberFilter>, ISubscriberFilterRepository
    {
        private readonly SubscriberContext _context;

        public SubscriberFilterRepository(SubscriberContext context) : base(context)
        {
            _context = context;
        }

        public IUnitOfWork UnitOfWork => _context;

        public IEnumerable<SubscriberFilterType> GetAllSubscriberFilterTypes()
        {
            return SubscriberFilterType.List();
        }
    }
}