using Notifications.Core.Mediator;
using Notifications.Core.SeedWork;
using System.Linq;
using System.Threading.Tasks;

namespace Notifying.API.Infrastructure
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediatorHandler mediator, NoSqlEntity noSqlEntity)
        {
            var domainEvents = noSqlEntity.DomainEvents;
            noSqlEntity.ClearDomainEvents();

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishDomainEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}