using EventBus.Abstractions;
using EventBus.Events;
using IntegrationEventLogEF;
using IntegrationEventLogEF.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Subscribing.Infrastructure;
using System;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace Subscribing.Application.IntegrationEvents
{
    public class SubscribingIntegrationEventService : ISubscribingIntegrationEventService
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly SubscriberContext _subscriberContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<SubscribingIntegrationEventService> _logger;

        public SubscribingIntegrationEventService(IEventBus eventBus,
            SubscriberContext notifyingContext,
            IntegrationEventLogContext eventLogContext,
             Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            ILogger<SubscribingIntegrationEventService> logger)
        {
            _subscriberContext = notifyingContext ?? throw new ArgumentNullException(nameof(notifyingContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_subscriberContext.Database.GetDbConnection());
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
        {
            var pendingLogEvents = await _eventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);

            foreach (var logEvt in pendingLogEvents)
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", logEvt.EventId, Assembly.GetExecutingAssembly().FullName, logEvt.IntegrationEvent);

                try
                {
                    await _eventLogService.MarkEventAsInProgressAsync(logEvt.EventId);
                    _eventBus.Publish(logEvt.IntegrationEvent);
                    await _eventLogService.MarkEventAsPublishedAsync(logEvt.EventId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "ERROR publishing integration event: {IntegrationEventId} from {AppName}", logEvt.EventId, Assembly.GetExecutingAssembly().FullName);

                    await _eventLogService.MarkEventAsFailedAsync(logEvt.EventId);
                }
            }
        }

        public async Task AddAndSaveEventAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- Enqueuing integration event {IntegrationEventId} to repository ({@IntegrationEvent})", evt.Id, evt);

            await _eventLogService.SaveEventAsync(evt, _subscriberContext.GetCurrentTransaction());
        }
    }
}