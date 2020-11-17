using Microsoft.EntityFrameworkCore;
using Notifying.Domain.Models.MessageAgregate;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Notifying.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NotificationContext _context;

        public SeedWork.IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public NotificationRepository(NotificationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Notification Add(Notification notificiation)
        {
            return _context.Notifications
                .Add(notificiation)
                .Entity;
        }

        public Notification Update(Notification notification)
        {
            return _context.Notifications
                    .Update(notification)
                    .Entity;
        }

        public async Task<Notification> FindByIdAsync(Guid id)
        {
            return await _context.Notifications
              .Where(b => b.Id == id)
              .SingleOrDefaultAsync();
        }
    }
}