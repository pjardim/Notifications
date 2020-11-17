using FluentValidation;
using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Commands
{
    public class SaveSentMassageCommand : Command
    {
        public SaveSentMassageCommand(Guid messageId, string senderPartyId, string recipientPartyId, string subject, string body, DateTime sentDate)
        {
            MessageId = messageId;
            SenderPartyId = senderPartyId;
            RecipientPartyId = recipientPartyId;
            Subject = subject;
            Body = body;
            SentDate = sentDate;
            AggregateId = MessageId;
        }

        public Guid MessageId { get; }
        public string SenderPartyId { get; set; }
        public string RecipientPartyId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime SentDate { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new SaveSentMassageCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddMailBoxItemCommand : Command
    {
        public AddMailBoxItemCommand(Guid messageId, string sendersPartyIds, string recipientPartyId, string subject, string body, bool requireAcknowledgement, DateTime sentDate)
        {
            MessageId = messageId;
            SendersPartyIds = sendersPartyIds;
            RecipientPartyId = recipientPartyId;
            Subject = subject;
            Body = body;
            SentDate = sentDate;
            RequireAcknowledgement = requireAcknowledgement;
            AggregateId = MessageId;
        }

        public Guid MessageId { get; private set; }
        public string SendersPartyIds { get; private set; }
        public string RecipientPartyId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public bool RequireAcknowledgement { get; private set; }
        public DateTime SentDate { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new AddMailBoxItemCommandCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SendDirectEmailCommand : Command
    {
        public SendDirectEmailCommand(Guid messageId, string sendersPartyId, string recipientPartyId, string subject, string body, bool requireAcknowledgement, DateTime sentDate)
        {
            MessageId = messageId;
            SendersPartyId = sendersPartyId;
            RecipientPartyId = recipientPartyId;
            Subject = subject;
            Body = body;
            SentDate = sentDate;
            RequireAcknowledgement = requireAcknowledgement;
            AggregateId = MessageId;
        }

        public Guid MessageId { get; private set; }
        public string SendersPartyId { get; private set; }
        public string RecipientPartyId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public bool RequireAcknowledgement { get; private set; }
        public DateTime SentDate { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new SendDirectEmailCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class MessageAcknowledgedCommand : Command
    {
        public MessageAcknowledgedCommand(Guid messageId, string partyId)
        {
            MessageId = messageId;
            PartyId = partyId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
        public string PartyId { get; private set; }

        public override bool IsValid()
        {
            return true;
        }
    }

    public class MessageMarkAsReadCommand : Command
    {
        public MessageMarkAsReadCommand(Guid messageId, string partyId)
        {
            MessageId = messageId;
            PartyId = partyId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
        public string PartyId { get; private set; }

        public override bool IsValid()
        {
            return true;
        }
    }


    public class MessageMoveToInboxCommand : Command
    {
        public MessageMoveToInboxCommand(Guid messageId, string partyId)
        {
            MessageId = messageId;
            PartyId = partyId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
        public string PartyId { get; private set; }

        public override bool IsValid()
        {
            return true;
        }
    }


    public class MessageMarkAsDeletedCommand : Command
    {
        public MessageMarkAsDeletedCommand(Guid messageId, string partyId)
        {
            MessageId = messageId;
            PartyId = partyId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
        public string PartyId { get; private set; }

        public override bool IsValid()
        {
            return true;
        }
    }

    public class MailReadCommand : Command
    {
        public MailReadCommand(Guid messageId, string partyId)
        {
            MessageId = messageId;
            PartyId = partyId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
        public string PartyId { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new MailReadCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class MailReadCommandValidation : AbstractValidator<MailReadCommand>
    {
        public MailReadCommandValidation()
        {
            RuleFor(c => c.MessageId)
              .NotEqual(Guid.Empty)
              .WithMessage("Massage Invalid");

            RuleFor(c => c.PartyId)
              .NotEmpty()
              .WithMessage("PartyId Invalid");
        }
    }

    public class SaveSentMassageCommandValidation : AbstractValidator<SaveSentMassageCommand>
    {
        public SaveSentMassageCommandValidation()
        {
            //TODO
        }
    }

    public class AddMailBoxItemCommandCommandValidation : AbstractValidator<AddMailBoxItemCommand>
    {
        public AddMailBoxItemCommandCommandValidation()
        {
            //TODO
        }
    }

    public class SendDirectEmailCommandValidation : AbstractValidator<SendDirectEmailCommand>
    {
        public SendDirectEmailCommandValidation()
        {
            //TODO
        }
    }
}