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
    public class SubscriberFilterCommandHanlder :
                IRequestHandler<AddSubscriberFilterCommand, bool>

    {
        private readonly ISubscriberFilterRepository _subscriberFilterRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public SubscriberFilterCommandHanlder(ISubscriberFilterRepository subscriberFilterRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _subscriberFilterRepository = subscriberFilterRepository ?? throw new ArgumentNullException(nameof(subscriberFilterRepository));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(AddSubscriberFilterCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var subscriberFilter = new SubscriberFilter(command.Id, command.ApplicationEventId,
             command.FilterType, command.FilterValue);

            if (await _subscriberFilterRepository.GetAsync(x => x.FilterType == command.FilterType && x.FilterValue == command.FilterValue) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Filter Type already exists with this Value"));
                return false;
            }

            _subscriberFilterRepository.Add(subscriberFilter);

            var result = await _subscriberFilterRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new SubscriberFilterCreatedEvent(subscriberFilter.Id, subscriberFilter.ApplicationEventId,
             subscriberFilter.FilterType, subscriberFilter.FilterValue));

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
            _subscriberFilterRepository.Dispose();
        }
    }
}