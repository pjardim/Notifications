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
    public class SubscriberApplicationEventCommandHandler :
              IRequestHandler<CreateSubscriberApplicationEventCommand, bool>,
              IRequestHandler<UpdateSubscriberApplicationEventCommand, bool>

    {
        private readonly ISubscriberApplicationEventRepository _subscriberApplicationEventRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public SubscriberApplicationEventCommandHandler(ISubscriberApplicationEventRepository subscriberApplicationEventRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _subscriberApplicationEventRepository = subscriberApplicationEventRepository ?? throw new ArgumentNullException(nameof(subscriberApplicationEventRepository));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(CreateSubscriberApplicationEventCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var subscriberApplicationEvent = new SubscriberApplicationEvent(command.SubscriberPartyId, command.ApplicationEventId,
             command.ChannelId);

            if (await _subscriberApplicationEventRepository.GetAsync(
                x => x.SubscriberPartyId == command.SubscriberPartyId
                && x.ApplicationEventId == command.ApplicationEventId) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Subscriber Application Event Already exists"));
                return false;
            }

            _subscriberApplicationEventRepository.Add(subscriberApplicationEvent);

            var result = await _subscriberApplicationEventRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new SubscriberApplicationEventCreatedEvent(subscriberApplicationEvent.SubscriberPartyId, subscriberApplicationEvent.ApplicationEventId, subscriberApplicationEvent.ChannelId));
            return result;
        }

        public async Task<bool> Handle(UpdateSubscriberApplicationEventCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var existingSubscriberApplicationEvent = await _subscriberApplicationEventRepository.
                GetAsync(x => x.SubscriberPartyId == command.SubscriberPartyId
                && x.ApplicationEventId == command.ApplicationEventId);

            var subscriberApplicationEvent = new SubscriberApplicationEvent(command.SubscriberPartyId, command.ApplicationEventId, command.ChannelId);

            if (existingSubscriberApplicationEvent == null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Subscriber Application Event was not found."));
                return false;
            }

            _subscriberApplicationEventRepository.Update(subscriberApplicationEvent);

            var result = await _subscriberApplicationEventRepository.UnitOfWork.SaveEntitiesAsync();
            if (result)
                await _mediatorHandler.Publish(new SubscriberApplicationEventUpdatedEvent(subscriberApplicationEvent.SubscriberPartyId, subscriberApplicationEvent.ApplicationEventId, subscriberApplicationEvent.ChannelId));
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
            _subscriberApplicationEventRepository.Dispose();
        }
    }
}