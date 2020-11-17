using Notifications.Core.Messaging;
using Notifications.Core.Messaging.CommonMessages.DomainEvents;
using Notifications.Core.Notifications;
using System.Threading.Tasks;

namespace Notifications.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task Publish<T>(T @event) where T : Event;

        Task<bool> Send<T>(T command) where T : Command;

        Task PublishNotification<T>(T notification) where T : DomainNotification;

        Task PublishDomainEvent<T>(T notification) where T : DomainEvent;
    }
}