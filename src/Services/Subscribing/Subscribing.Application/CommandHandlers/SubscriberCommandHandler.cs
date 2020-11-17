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
    public class SubscriberCommandHandler :
              IRequestHandler<CreateSubscriberCommand, bool>,
              IRequestHandler<UpdateSubscriberCommand, bool>,
              IRequestHandler<RemoveSubscriberCommand, bool>

    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public SubscriberCommandHandler(ISubscriberRepository subscriberRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _subscriberRepository = subscriberRepository ?? throw new ArgumentNullException(nameof(subscriberRepository));

            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(CreateSubscriberCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            if (_subscriberRepository.Get(x => x.SubscriberPartyId == command.SubscriberPartyId) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Subscriber Id already been taken."));
                return false;
            }

            var subscriber = new Subscriber(command.SubscriberPartyId, command.Email, command.Name);

            _subscriberRepository.Add(subscriber);

            var result = await _subscriberRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new SubscriberCreatedEvent(subscriber.SubscriberPartyId, subscriber.Email, subscriber.Name));

            return result;
        }

        public async Task<bool> Handle(UpdateSubscriberCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var existingSubscriber = await _subscriberRepository.GetAsync(x => x.SubscriberPartyId == command.SubscriberPartyId);

            var subscriberUpdated = new Subscriber(command.SubscriberPartyId, command.Email, command.Name);

            if (existingSubscriber != null && existingSubscriber.Id != subscriberUpdated.Id)
                if (!existingSubscriber.Equals(subscriberUpdated))
                {
                    await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType,
                        "The Subscriber partyId has already been taken!"));
                    return false;
                }

            _subscriberRepository.Update(subscriberUpdated);

            var result = await _subscriberRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new SubscriberUpdatedEvent(subscriberUpdated.SubscriberPartyId,
                    subscriberUpdated.Email, subscriberUpdated.Name));

            return result;
        }

        public async Task<bool> Handle(RemoveSubscriberCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var subscriberToDelete = await _subscriberRepository.GetByIdAsync(command.SubscriberPartyId);

            _subscriberRepository.Remove(subscriberToDelete.SubscriberPartyId);

            var result = await _subscriberRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new SubscriberDeletedEvent(subscriberToDelete.SubscriberPartyId,
                    subscriberToDelete.Email, subscriberToDelete.Name));

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
            _subscriberRepository.Dispose();
        }
    }
}