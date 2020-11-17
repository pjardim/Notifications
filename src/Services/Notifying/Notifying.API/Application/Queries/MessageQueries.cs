using Notifying.API.Infrastructure.Repositories;
using Notifying.API.Model;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Notifying.API.Application.Queries
{
    public class MessageQueries : IMessageQueries
    {
        private readonly NotificationsNoSqlRepository _messageRepository;

        public MessageQueries(NotificationsNoSqlRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<IEnumerable<Message>> GetPendingMessagesWithNotificationsAsync()
        {
            return await _messageRepository.GetPendingMessagesWithNotificationsAsync();
        }
    }
}