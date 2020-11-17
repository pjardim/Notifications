using EventBus.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Subscribing.SignalrHub.IntegrationEvents.Events;
using Subscribing.SignalrHub.SignalrHub;
using System;
using System.Threading.Tasks;

namespace Subscribing.SignalrHub.IntegrationEvents.EventHandling
{
    public class MailCreatedIntegrationEventHandler : IIntegrationEventHandler<MailCreatedIntegrationEvent>
    {
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly ILogger<MailCreatedIntegrationEventHandler> _logger;

        public MailCreatedIntegrationEventHandler(IHubContext<NotificationsHub> hubContext,
          ILogger<MailCreatedIntegrationEventHandler> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(MailCreatedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}- "))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                await _hubContext.Clients
                    .Group(@event.RecipientPartyId)
                    .SendAsync("Mail Created", new { MessageId = @event.MessageId, From = @event.SenderPartyIds });
            }
        }
    }
}