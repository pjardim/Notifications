using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface ISubscriberFilterRepository : IRepository<SubscriberFilter>, IRepositoryBase<SubscriberFilter>
    {
        IEnumerable<SubscriberFilterType> GetAllSubscriberFilterTypes();
    }
}
