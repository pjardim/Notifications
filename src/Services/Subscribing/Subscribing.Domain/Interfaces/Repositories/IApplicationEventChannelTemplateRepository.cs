using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Subscribing.Domain.Interfaces.Repositories
{
    public interface IApplicationEventChannelTemplateRepository : IRepository<ApplicationEventChannelTemplate>, IRepositoryBase<ApplicationEventChannelTemplate>
    {
        Task<ApplicationEventChannelTemplate> GetByApplicationEventAndChannel(Guid applicationEventId, Guid channelId);
        Task<ApplicationEventChannelTemplate> GetByApplicationEventAndChannel(Guid applicationEventId, string messageChannel);
    }
}
