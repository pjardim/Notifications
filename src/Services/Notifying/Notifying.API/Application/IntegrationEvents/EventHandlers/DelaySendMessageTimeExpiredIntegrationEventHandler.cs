using EventBus.Abstractions;
using EventBus.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Notifications.API;
using Notifying.API.Application.Commands;
using Notifying.API.Application.IntegrationEvents.Events;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace Notifying.API.Application.IntegrationEvents.EventHandlers
{
    public class DelaySendMessageTimeExpiredIntegrationEventHandler
        : IIntegrationEventHandler<DelaySendMessageTimeExpiredIntegrationEvent>
    {
        private readonly ILogger<DelaySendMessageTimeExpiredIntegrationEventHandler> _logger;

        private readonly IMediator _mediatorHandler;

        public DelaySendMessageTimeExpiredIntegrationEventHandler(IMediator mediatorHandler
            , ILogger<DelaySendMessageTimeExpiredIntegrationEventHandler> logger)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(DelaySendMessageTimeExpiredIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var result = false;

                var setDelaySendMessageTimeExpiredCommand = new SetDelaySendMessageTimeExpiredCommand(@event.MessageId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    setDelaySendMessageTimeExpiredCommand.GetGenericTypeName(),
                    nameof(setDelaySendMessageTimeExpiredCommand.MessageId),
                    setDelaySendMessageTimeExpiredCommand.MessageId,
                    setDelaySendMessageTimeExpiredCommand);

                await _mediatorHandler.Send(setDelaySendMessageTimeExpiredCommand);

                var sendMessageCommand = new StartSendingMessageCommand(@event.MessageId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    sendMessageCommand.GetGenericTypeName(),
                    nameof(sendMessageCommand.MessageId),
                    sendMessageCommand.MessageId,
                    sendMessageCommand);

                result = await _mediatorHandler.Send(sendMessageCommand);
            }
        }

        //public async Task Handle(GenericEvent @event)
        //{
        //    // await _mediatorHandler.SendCommand(new CreateNotificationCommand { AggregateId = new Guid(), Comment = @event.CrewComment, app = @event.GetType().Name });
        //}
    }
}