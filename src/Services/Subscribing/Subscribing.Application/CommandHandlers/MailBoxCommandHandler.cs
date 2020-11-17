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
    internal class MailBoxCommandHandler :
              IRequestHandler<AddMailBoxItemCommand, bool>,
              IRequestHandler<MailReadCommand, bool>,
              IRequestHandler<MessageAcknowledgedCommand, bool>,
              IRequestHandler<MessageMarkAsDeletedCommand, bool>,
              IRequestHandler<MessageMarkAsReadCommand, bool>,
              IRequestHandler<SendDirectEmailCommand, bool>,
              IRequestHandler<MessageMoveToInboxCommand, bool>

    {
        private readonly IMailBoxRepository _messageRepository;
        private readonly IMediatorHandler _mediatorHandler;

        public MailBoxCommandHandler(IMailBoxRepository messageRepository,
                                      IMediatorHandler mediatorHandler)
        {
            _messageRepository = messageRepository ?? throw new ArgumentNullException(nameof(messageRepository));

            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<bool> Handle(AddMailBoxItemCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            if (_messageRepository.Get(x => x.MessageId == command.MessageId && x.RecipientPartyId == command.RecipientPartyId) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Parameter Name already been taken."));
            }

            var mailBoxItem = new MailBoxItem(command.MessageId, command.RecipientPartyId, command.SendersPartyIds, command.Subject, command.Body, command.RequireAcknowledgement);

            _messageRepository.Add(mailBoxItem);

            var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new MailBoxItemCreatedEvent(mailBoxItem.MessageId, mailBoxItem.RecipientPartyId,
                    mailBoxItem.SenderPartyIds, mailBoxItem.Subject, mailBoxItem.Body, mailBoxItem.Read, mailBoxItem.Deleted, mailBoxItem.Excluded, mailBoxItem.CreatedDate));

            return result;
        }

        public async Task<bool> Handle(SendDirectEmailCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            if (_messageRepository.Get(x => x.MessageId == command.MessageId && x.RecipientPartyId == command.RecipientPartyId) != null)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, "The Parameter Name already been taken."));
            }

            var mailBoxItem = new MailBoxItem(command.MessageId, command.RecipientPartyId, command.SendersPartyId, command.Subject, command.Body, command.RequireAcknowledgement);
            mailBoxItem.SetDirectEmail();

            _messageRepository.Add(mailBoxItem);

            var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new DirectEmailSentEvent(mailBoxItem.MessageId, mailBoxItem.RecipientPartyId,
                    mailBoxItem.SenderPartyIds, mailBoxItem.Subject, mailBoxItem.Body, mailBoxItem.Read, mailBoxItem.Deleted, mailBoxItem.Excluded, mailBoxItem.CreatedDate));

            return result;
        }

        public async Task<bool> Handle(MailReadCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var mailBoxItem = await _messageRepository.GetWithRelatedByIdAsync(command.MessageId, command.PartyId);

            if (mailBoxItem.Read)
                return true;

            mailBoxItem.SetReadEmail();
            _messageRepository.Update(mailBoxItem);

            var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new MailReadEvent(mailBoxItem.MessageId));

            return result;
        }

        public async Task<bool> Handle(MessageAcknowledgedCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var mailBoxItem = await _messageRepository.GetByIdAsync(command.MessageId, command.PartyId);

            mailBoxItem.SetAcknowledged();

            _messageRepository.Update(mailBoxItem);

            var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new MessageAcknowledgedEvent(mailBoxItem.MessageId));

            return result;
        }

        public async Task<bool> Handle(MessageMarkAsDeletedCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var mailBoxItem = await _messageRepository.GetByIdAsync(command.MessageId, command.PartyId);

            mailBoxItem.SetDeletedMail();

            _messageRepository.Update(mailBoxItem);

            var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new MessageMarkedAsDeletedEvent(mailBoxItem.MessageId));

            return result;
        }

        public async Task<bool> Handle(MessageMarkAsReadCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var mailBoxItem = await _messageRepository.GetByIdAsync(command.MessageId, command.PartyId);

            mailBoxItem.SetReadEmail();

            _messageRepository.Update(mailBoxItem);

            var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new MessageMarkedAsReadEvent(mailBoxItem.MessageId));

            return result;
        }

        public async Task<bool> Handle(MessageMoveToInboxCommand command, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(command)) return false;

            var mailBoxItem = await _messageRepository.GetByIdAsync(command.MessageId, command.PartyId);

            mailBoxItem.MoveToInbox();

            _messageRepository.Update(mailBoxItem);

            var result = await _messageRepository.UnitOfWork.SaveEntitiesAsync();

            if (result)
                await _mediatorHandler.Publish(new MessageMovedToInboxEvent(mailBoxItem.MessageId));

            return result;
        }

        private bool ValidateCommand(Command command)
        {
            if (command.IsValid()) return true;

            foreach (var error in command.ValidationResult.Errors)
            {
                _mediatorHandler.PublishNotification(new DomainNotification(command.DomainMessageType, error.ErrorMessage));
            }

            return false;
        }

        public void Dispose()
        {
            _messageRepository.Dispose();
        }
    }
}