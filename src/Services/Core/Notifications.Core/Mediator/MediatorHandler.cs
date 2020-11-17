using MediatR;
using Notifications.Core.Data.EventSourcing;
using Notifications.Core.Messaging;
using Notifications.Core.Messaging.CommonMessages.DomainEvents;
using Notifications.Core.Notifications;
using System.Threading.Tasks;

namespace Notifications.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventSourcingRepository _eventSourcingRepository;

        public MediatorHandler(IMediator mediator, IEventSourcingRepository eventSourcingRepository)
        {
            _mediator = mediator;
            _eventSourcingRepository = eventSourcingRepository;
        }

        public async Task<bool> Send<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }

        public async Task Publish<T>(T @event) where T : Event
        {
            await _mediator.Publish(@event);
            await _eventSourcingRepository.SaveEvent(@event);
        }

        public async Task PublishNotification<T>(T notification) where T : DomainNotification
        {
            await _mediator.Publish(notification);
        }

        public async Task PublishDomainEvent<T>(T notification) where T : DomainEvent
        {
            await _mediator.Publish(notification);
        }
    }
}