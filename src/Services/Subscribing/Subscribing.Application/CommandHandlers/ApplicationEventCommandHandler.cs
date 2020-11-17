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
    public class ApplicationEventCommandHandler :
              IRequestHandler<CreateApplicationEventCommand, bool>,
              IRequestHandler<UpdateApplicationEventCommand, bool>,
              IRequestHandler<RemoveApplicationEventCommand, bool>

    {
        private readonly IApplicationEventRepository _applicationEventRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ApplicationEventCommandHandler(IApplicationEventRepository applicationEventRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _applicationEventRepository = applicationEventRepository ?? throw new ArgumentNullException(nameof(applicationEventRepository));

            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(CreateApplicationEventCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            if (_applicationEventRepository.Get(x => x.ApplicationEventName == command.ApplicationEventName) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The ApplicationEvent Name already been taken."));
                return false;
            }

            var applicationEvent = new ApplicationEvent(Guid.NewGuid(), command.ApplicationEventName, command.Description);

           

            _applicationEventRepository.Add(applicationEvent);

            var result = await _applicationEventRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new ApplicationEventCreatedEvent(applicationEvent.Id, applicationEvent.ApplicationEventName, applicationEvent.Description));

            return result;
        }

        public async Task<bool> Handle(UpdateApplicationEventCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var existingapplicationEvent = await _applicationEventRepository.GetAsync(x => x.ApplicationEventName == command.ApplicationEventName);

            var applicationEventUpdated = new ApplicationEvent(command.Id, command.ApplicationEventName, command.Description);

            if (existingapplicationEvent != null && existingapplicationEvent.Id != applicationEventUpdated.Id)
                if (!existingapplicationEvent.Equals(applicationEventUpdated))
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Application Event has already been taken!"));
                    return false;
                }

            _applicationEventRepository.Update(applicationEventUpdated);

            var result = await _applicationEventRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new ApplicationEventUpdatedEvent(applicationEventUpdated.Id, applicationEventUpdated.ApplicationEventName, applicationEventUpdated.Description));

            return result;
        }

        public async Task<bool> Handle(RemoveApplicationEventCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var applicationEventToDelete = _applicationEventRepository.GetById(command.Id);

            _applicationEventRepository.Remove(applicationEventToDelete.Id);

            var result = await _applicationEventRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new ApplicationEventDeletedEvent(applicationEventToDelete.Id, applicationEventToDelete.Description, applicationEventToDelete.ApplicationEventName));

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
            _applicationEventRepository.Dispose();
        }
    }
}