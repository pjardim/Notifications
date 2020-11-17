using MediatR;
using Notifications.Core.Mediator;
using Notifications.Core.Messaging;
using Notifications.Core.Notifications;
using Subscribing.Application.Commands;
using Subscribing.Application.Events;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Subscribing.Application.CommandHandlers
{
    public class ApplicationEventChannelCommandHandler :
               IRequestHandler<CreateApplicationEventChannelCommand, bool>,
         IRequestHandler<UpdateApplicationEventChannelCommand, bool>

    {
        private readonly IApplicationEventChannelRepository _applicationEventChannelRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ApplicationEventChannelCommandHandler(IApplicationEventChannelRepository applicationEventChannelRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _applicationEventChannelRepository = applicationEventChannelRepository ?? throw new ArgumentNullException(nameof(applicationEventChannelRepository));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(CreateApplicationEventChannelCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var applicationEventChannel = new ApplicationEventChannel(command.ApplicationEventId, command.ChannelId,
             command.DelayedSendMinutes, command.Enabled, command.RequireAcknowledgement);

            if (await _applicationEventChannelRepository.GetAsync(x => x.ApplicationEventId == command.ApplicationEventId && x.ChannelId == command.ChannelId) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Application Event Channel Already exists"));
                return false;
            }

            _applicationEventChannelRepository.Add(applicationEventChannel);

            var result = await _applicationEventChannelRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new ApplicationEventChannelCreatedEvent(command.ApplicationEventId, command.ChannelId, command.DelayedSendMinutes, command.Enabled));
            return result;
        }

        public async Task<bool> Handle(UpdateApplicationEventChannelCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var existingApplicationEventChannel = await _applicationEventChannelRepository.
                GetAsync(x => x.ApplicationEventId == command.ApplicationEventId && x.ChannelId == command.ChannelId);

            var applicationEventChannel = new ApplicationEventChannel(command.ApplicationEventId, command.ChannelId,
             command.DelayedSendMinutes, command.Enabled, command.RequireAcknowledgement);

            if (existingApplicationEventChannel == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Application Event Channel was not found."));
                return false;
            }

            _applicationEventChannelRepository.Update(applicationEventChannel);

            var result = await _applicationEventChannelRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new ApplicationEventChannelUpdatedEvent(command.ApplicationEventId, command.ChannelId, command.DelayedSendMinutes, command.Enabled));
            return result;
        }

        private bool ValidateCommand(Command message)
        {
            if (message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotification(new DomainNotification(message.DomainMessageType, error.ErrorMessage));
            }

            return false;
        }

        public void Dispose()
        {
            _applicationEventChannelRepository.Dispose();
        }
    }
}