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
    internal class ApplicationEventChannelTemplateCommandHandler :
              IRequestHandler<CreateApplicationEventChannelTemplateCommand, bool>,
              IRequestHandler<UpdateApplicationEventChannelTemplateCommand, bool>

    {
        private readonly IApplicationEventChannelTemplateRepository _applicationEventChannelTemplateRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public ApplicationEventChannelTemplateCommandHandler(IApplicationEventChannelTemplateRepository applicationEventChannelTemplateRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _applicationEventChannelTemplateRepository = applicationEventChannelTemplateRepository ?? throw new ArgumentNullException(nameof(applicationEventChannelTemplateRepository));

            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(CreateApplicationEventChannelTemplateCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            if (await _applicationEventChannelTemplateRepository.GetAsync(x => x.ApplicationEventId == command.ApplicationEventId && x.ChannelId == command.ChannelId) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Application Event Channel Template already exists for this channel."));
            }

            var applicationEventChannelTemplate = new ApplicationEventChannelTemplate(command.ApplicationEventId, command.ChannelId,
                command.Format, command.Encoding, command.Subject, command.Body);

            _applicationEventChannelTemplateRepository.Add(applicationEventChannelTemplate);


            var result = await _applicationEventChannelTemplateRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new ApplicationEventChannelTemplateCreatedEvent(command.ApplicationEventId, command.ChannelId, command.Format, command.Encoding, command.Subject, command.Body));
            return result;
        }

        public async Task<bool> Handle(UpdateApplicationEventChannelTemplateCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var existingApplicationEventChannelTemplate = await _applicationEventChannelTemplateRepository.GetAsync(x => x.ApplicationEventId == command.ApplicationEventId && x.ChannelId == command.ChannelId);

            if (existingApplicationEventChannelTemplate == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Application Event Template was not found for this channel"));
                return false;
            }

            var applicationEventChannelTemplate = new ApplicationEventChannelTemplate(command.ApplicationEventId, command.ChannelId,
                command.Format, command.Encoding, command.Subject, command.Body);



            _applicationEventChannelTemplateRepository.Update(applicationEventChannelTemplate);


            var result = await _applicationEventChannelTemplateRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new ApplicationEventChannelTemplateCreatedEvent(command.ApplicationEventId, command.ChannelId, command.Format, command.Encoding, command.Subject, command.Body));
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
            _applicationEventChannelTemplateRepository.Dispose();
        }
    }
}