using Notifying.API.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifying.API.Infrastructure.Repositories
{
    public interface INotificationsNoSqlRepository
    {
        Task AddMessage(Message notification);

        Task UpdateMessage(Message notification);

        Task<Message> GetMesssageAsync(Guid id);

        Task<IEnumerable<Message>> GetPendingMessagesWithNotificationsAsync();

        Task<Message> GetPendingMessagesByPartyIdAndApplicationEventGroup(List<string> listSubscribersId, string applicationEventName);
    }
}