using EventBus.Abstractions;
using EventBus.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Notifications.Core.Mediator;
using Serilog.Context;
using Subscribing.Application.Commands;
using Subscribing.Application.IntegrationEvents.Events;
using System;
using System.Threading.Tasks;

namespace Subscribing.Application.IntegrationEvents.EventHandling
{
    public class MessageSentIntegrationEventHandler
        : IIntegrationEventHandler<MessageSentIntegrationEvent>
    {
        private readonly ILogger<MessageSentIntegrationEvent> _logger;

        private readonly IMediatorHandler _mediator;

        public MessageSentIntegrationEventHandler(IMediatorHandler mediatorHandler
            , ILogger<MessageSentIntegrationEvent> logger)
        {
            _mediator = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(MessageSentIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{"Subscribing.Application"}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, "Subscribing.Application", @event);

                var saveSentMassageCommand = new AddMailBoxItemCommand(@event.MessageId, @event.SendersPartyIds, @event.RecipientPartyId,
                    @event.Subject, @event.Body, @event.RequireAcknowledgement, @event.SentDate); ;

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    saveSentMassageCommand.GetGenericTypeName(),
                    nameof(saveSentMassageCommand.MessageId),
                    saveSentMassageCommand.MessageId,
                    saveSentMassageCommand);

                await _mediator.Send(saveSentMassageCommand);
            }
        }
    }
}