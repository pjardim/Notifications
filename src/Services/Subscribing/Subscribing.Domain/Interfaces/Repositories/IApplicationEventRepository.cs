using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface IApplicationEventRepository : IRepository<ApplicationEvent>, IRepositoryBase<ApplicationEvent>
    {

        Task<ApplicationEvent> GetApplicationEventWithRelatedAsync(Guid applicationEventId);

        Task<IEnumerable<SubscriberFilter>> GetAllSubscriberFiltersAsync();
        Task<ApplicationEvent> GetApplicationEventByNameAsync(string applicationEventName);
        Task<IEnumerable<ApplicationEvent>> GetAllApplicationEventsWithRelatedAsync();
    }
}