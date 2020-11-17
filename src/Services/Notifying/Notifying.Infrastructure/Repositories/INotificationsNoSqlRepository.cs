using Notifying.Domain.Models.MessageAgregate;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifying.Infrastructure.Repositories
{
    public interface INotificationsNoSqlRepository
    {
        Task AddNotification(Notification notification);

        Task UpdateNotification(Notification notification);

        Task AddNotificationSubscribers(NotificationSubscriber messageNotificationSubscriber);

        Task<Notification> FindNotificationByIdAsync(Guid id);

        Task AddMessage(Message notification);

        Task UpdateMessage(Message notification);

        Task<Message> GetMesssageAsync(Guid id);

        Task<IEnumerable<Message>> GetPendingMessagesWithNotificationsAsync();

        Task<Message> GetPendingMessagesByPartyIdAndApplicationEventGroup(IEnumerable<string> listSubscribersId, string applicationEventName);
        Task<IEnumerable<Notification>> GetNotifications();
    }
}