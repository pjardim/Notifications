using MediatR;
using Microsoft.Extensions.Logging;
using Notifications.Core.Mediator;
using Notifying.API.Application.Commands;
using Notifying.API.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notifying.API.Application.CommandHandlers
{
    public class GroupNotificationCommandHandler : IRequestHandler<GroupMessageNotificationCommand, bool>
    {
        private readonly IMediatorHandler _mediatorHandler;
        private readonly ILogger<GroupNotificationCommandHandler> _logger;

        public GroupNotificationCommandHandler(IMediatorHandler mediatorHandler,
            ILogger<GroupNotificationCommandHandler> logger)
        {
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(GroupMessageNotificationCommand command, CancellationToken cancellationToken)
        {
            var messageNotification = new Notification(Guid.NewGuid(), command.MessageId, command.PublisherPartyId,
               command.ApplicationEventId, command.ApplicationEventName, command.DelaySendMinutes, command.PayLoad);

            messageNotification.AddNotificationSubscribers(command.RecipientPartyIds);

            _logger.LogInformation("----- Grouping Notification : {@MessageNotification}", messageNotification);

            return await _mediatorHandler.Send(new AddNotificationToMessageCommand(command.MessageId, messageNotification));
        }
    }
}