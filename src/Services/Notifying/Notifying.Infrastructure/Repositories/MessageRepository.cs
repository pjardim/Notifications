using Microsoft.EntityFrameworkCore;
using Notifying.Domain.Models.MessageAgregate;
using Notifying.Infrastructure.SeedWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifying.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly NotificationContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public MessageRepository(NotificationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Message Add(Message message)
        {
            return _context.Messages
                .Add(message)
                .Entity;
        }

        public void AddMessageNotification(Notification messageNotification)
        {
            _context.Notifications.Add(messageNotification);
        }

        public void AddNotificationSubscribers(NotificationSubscriber messageNotificationSubscriber)
        {
            _context.NotificationSubscribers.Add(messageNotificationSubscriber);
        }

        public void Update(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;
        }

        public async Task<Message> GetAsync(Guid messageId)
        {
            var message = await _context.Messages
              .Include(m => m.Notifications)
              .ThenInclude(mn => mn.NotificationSubscribers)
              .Where(b => b.Id == messageId)
              .FirstOrDefaultAsync();

            if (message == null)
            {
                message = _context
                            .Messages
                            .Local
                            .FirstOrDefault(o => o.Id == messageId);
            }
            if (message != null)
            {
                await _context.Entry(message)
                    .Collection(i => i.Notifications).LoadAsync();
                await _context.Entry(message)
                    .Reference(i => i.MessageStatus).LoadAsync();
            }

            return message;
        }

        public async Task<IEnumerable<Message>> GetPendingMessagesWithNotificationsAsync()
        {
            return await _context.Messages
                .Include(n => n.Notifications)
                 .Where(b => b.MessageStatus.Id == MessageStatus.Pending.Id).ToListAsync();
        }

        public async Task<Message> GetPendingMessagesByPartyIdAndApplicationEventGroup(IEnumerable<string> listSubscribersId, string applicationEventName)
        {
            return await _context.Messages
                 .Where(m => m.MessageStatus.Id == MessageStatus.Pending.Id
                 &&
                 (m.Notifications.Any(mn => mn.ApplicationEventName == applicationEventName))).FirstOrDefaultAsync();
        }
    }
}