using Notifications.Core.SeedWork;

namespace Notifying.Infrastructure.SeedWork
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}