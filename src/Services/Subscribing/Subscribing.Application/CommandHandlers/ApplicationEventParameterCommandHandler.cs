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
    public class ApplicationEventParameterCommandHandler :
              IRequestHandler<CreateApplicationEventParameterCommand, bool>,
              IRequestHandler<UpdateApplicationEventParameterCommand, bool>,
              IRequestHandler<RemoveApplicationEventParameterCommand, bool>

    {
        private readonly IApplicationEventParameterRepository _applicationEventParameterRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ApplicationEventParameterCommandHandler(IApplicationEventParameterRepository applicationEventParameterRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _applicationEventParameterRepository = applicationEventParameterRepository ?? throw new ArgumentNullException(nameof(applicationEventParameterRepository));

            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(CreateApplicationEventParameterCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            if (_applicationEventParameterRepository.Get(x => x.ParameterName == command.ParameterName) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Parameter Name already been taken."));
            }

            var applicationEventParameter = new ApplicationEventParameter(Guid.NewGuid(), command.ApplicationEventId, command.ParameterName, command.Description);

            _applicationEventParameterRepository.Add(applicationEventParameter);

            var result = await _applicationEventParameterRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new ApplicationEventParameterCreatedEvent(Guid.NewGuid(),
                applicationEventParameter.ApplicationEventId, applicationEventParameter.ParameterName, applicationEventParameter.Description));

            return result;
        }

        public async Task<bool> Handle(UpdateApplicationEventParameterCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var existingapplicationEventParameter = await _applicationEventParameterRepository.GetAsync(x => x.ParameterName == command.ParameterName
            && x.ApplicationEventId == command.ApplicationEventId);

            var applicationEventParameterUpdated = new ApplicationEventParameter(command.Id, command.ApplicationEventId, command.ParameterName, command.Description);

            if (existingapplicationEventParameter != null && existingapplicationEventParameter.Id != applicationEventParameterUpdated.Id)
                if (!existingapplicationEventParameter.Equals(applicationEventParameterUpdated))
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Application Event has already been taken!"));
                    return false;
                }

            _applicationEventParameterRepository.Update(applicationEventParameterUpdated);

            var result = await _applicationEventParameterRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new ApplicationEventParameterUpdatedEvent(Guid.NewGuid(),
                applicationEventParameterUpdated.ApplicationEventId, applicationEventParameterUpdated.ParameterName, applicationEventParameterUpdated.Description));

            return result;
        }

        public async Task<bool> Handle(RemoveApplicationEventParameterCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var applicationEventParameterToDelete = _applicationEventParameterRepository.GetById(command.Id);

            _applicationEventParameterRepository.Remove(applicationEventParameterToDelete.Id);

            var result = await _applicationEventParameterRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new ApplicationEventParameterDeletedEvent(Guid.NewGuid(),
                applicationEventParameterToDelete.ApplicationEventId, applicationEventParameterToDelete.ParameterName, applicationEventParameterToDelete.Description));

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
            _applicationEventParameterRepository.Dispose();
        }
    }
}