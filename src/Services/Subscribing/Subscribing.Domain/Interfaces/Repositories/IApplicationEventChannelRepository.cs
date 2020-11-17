using Notifications.Core.SeedWork;
using System;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface IApplicationEventChannelRepository : IRepository<ApplicationEventChannel>, IRepositoryBase<ApplicationEventChannel>
    {
        Task<ApplicationEventChannel> GetByApplicationEventIdAndChannelAsync(Guid applicationEventId, Guid channelId);
    }
}