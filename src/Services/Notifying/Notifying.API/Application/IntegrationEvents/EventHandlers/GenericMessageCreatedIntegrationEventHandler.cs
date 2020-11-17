using EventBus.Abstractions;
using Notifications.Core.ApplicationEvents;
using Notifications.Core.Mediator;
using Notifying.API.Application.Commands;
using System;
using System.Threading.Tasks;

namespace Notifying.API.Application.IntegrationEvents.EventHandlers
{
    public class GenericMessageCreatedIntegrationEventHandler : IIntegrationEventHandler<GenericApplicationEvent>

    {
        // private readonly ILogger<BackOfficeCommentCreatedIntegrationEventHandler> _logger;

        private readonly IMediatorHandler _mediatorHandler;

        public GenericMessageCreatedIntegrationEventHandler(IMediatorHandler mediatorHandler)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task Handle(GenericApplicationEvent @event)
        {
            await _mediatorHandler.Send(new StartGenericEventProcessCommand(@event));
        }
    }
}