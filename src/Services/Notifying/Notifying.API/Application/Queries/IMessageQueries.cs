using Notifying.API.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifying.API.Application.Queries
{
    public interface IMessageQueries
    {
        Task<IEnumerable<Message>> GetPendingMessagesWithNotificationsAsync();
    }
}