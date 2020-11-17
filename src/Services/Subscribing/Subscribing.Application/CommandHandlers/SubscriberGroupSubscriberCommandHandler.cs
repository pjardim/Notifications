using MediatR;
using Notifications.Core.Mediator;
using Notifications.Core.Messaging;
using Notifications.Core.Notifications;
using Subscribing.Application.Commands;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Subscribing.Application.CommandHandlers
{
    internal class SubscriberGroupSubscriberCommandHandler :
              IRequestHandler<SaveSubscriberGroupSubscriberCommand, bool>

    {
        private readonly ISubscriberGroupSubscriberRepository _subscriberGroupSubscriberRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public SubscriberGroupSubscriberCommandHandler(ISubscriberGroupSubscriberRepository subscriberGroupSubscriberRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _subscriberGroupSubscriberRepository = subscriberGroupSubscriberRepository ?? throw new ArgumentNullException(nameof(subscriberGroupSubscriberRepository));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(SaveSubscriberGroupSubscriberCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var newUserGroups = command.SubscriberGroups ?? new List<int>();

            var currentUserGroups = await _subscriberGroupSubscriberRepository.GetAllByPartyIdAsync(command.SubscriberPartyId);
            var currentGroupIds = currentUserGroups.Select(x => x.SubscriberGroupId).ToList();

            var groupsToExclude = currentGroupIds.Except(newUserGroups);
            var groupsToInclude = newUserGroups?.Except(currentGroupIds).ToList();

            foreach (var group in groupsToExclude)
            {
                var subscriber = new SubscriberGroupSubscriber(command.SubscriberPartyId, group);
                _subscriberGroupSubscriberRepository.Remove(subscriber);
            }

            foreach (var group in groupsToInclude)
            {
                var subscriber = new SubscriberGroupSubscriber(command.SubscriberPartyId, group);
                _subscriberGroupSubscriberRepository.Add(subscriber);
            }

            var result = await _subscriberGroupSubscriberRepository.UnitOfWork.SaveEntitiesAsync();

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
            _subscriberGroupSubscriberRepository.Dispose();
        }
    }
}