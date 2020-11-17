

using Notifying.Domain.Models;
using Notifying.Domain.Models.MessageAgregate;
using Notifying.Infrastructure.SeedWork;
using System;
using System.Threading.Tasks;

namespace Notifying.Infrastructure.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Notification Add(Notification notification);
        Notification Update(Notification notification);
        Task<Notification> FindByIdAsync(Guid id);
        
    }
}