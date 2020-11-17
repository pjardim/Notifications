using Notifications.Core.Mediator;
using Notifications.Core.SeedWork;
using System.Linq;
using System.Threading.Tasks;

namespace Subscribing.Infrastructure
{
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediatorHandler mediator, SubscriberContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublishDomainEvent(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}