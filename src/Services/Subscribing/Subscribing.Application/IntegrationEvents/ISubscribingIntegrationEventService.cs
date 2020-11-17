using EventBus.Events;
using System;
using System.Threading.Tasks;

namespace Subscribing.Application.IntegrationEvents
{
    public interface ISubscribingIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);

        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}