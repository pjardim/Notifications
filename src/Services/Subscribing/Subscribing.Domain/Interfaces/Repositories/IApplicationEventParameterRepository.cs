using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface IApplicationEventParameterRepository : IRepository<ApplicationEventParameter>, IRepositoryBase<ApplicationEventParameter>
    {
        Task<IEnumerable<ApplicationEventParameter>> GetByApplicationEventIdAsync(Guid applicationEventId);
        Task<ApplicationEventParameter> GetByIdWithRelatedAsync(Guid applicationEventParameterId);
    }
}