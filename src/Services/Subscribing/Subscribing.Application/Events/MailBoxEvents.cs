using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Events
{
    public class MailBoxItemCreatedEvent : Event
    {
        public MailBoxItemCreatedEvent(Guid messageId, string recipientPartyId, string senderPartyIds, string subject, string body,
            bool read, bool deleted, bool excluded, DateTime createdDate)
        {
            MessageId = messageId;
            RecipientPartyId = recipientPartyId;
            SenderPartyIds = senderPartyIds;
            Subject = subject;
            Body = body;
            Read = read;
            Deleted = deleted;
            Excluded = excluded;
            CreatedDate = createdDate;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
        public string RecipientPartyId { get; private set; }
        public string SenderPartyIds { get; private set; }

        public string Subject { get; private set; }
        public string Body { get; private set; }

        public bool Read { get; private set; }
        public bool Deleted { get; private set; }
        public bool Excluded { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }

    public class DirectEmailSentEvent : Event
    {
        public DirectEmailSentEvent(Guid messageId, string recipientPartyId, string senderPartyIds, string subject, string body,
            bool read, bool deleted, bool excluded, DateTime createdDate)
        {
            MessageId = messageId;
            RecipientPartyId = recipientPartyId;
            SenderPartyIds = senderPartyIds;
            Subject = subject;
            Body = body;
            Read = read;
            Deleted = deleted;
            Excluded = excluded;
            CreatedDate = createdDate;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
        public string RecipientPartyId { get; private set; }
        public string SenderPartyIds { get; private set; }

        public string Subject { get; private set; }
        public string Body { get; private set; }

        public bool Read { get; private set; }
        public bool Deleted { get; private set; }
        public bool Excluded { get; private set; }
        public DateTime CreatedDate { get; private set; }
    }

    public class MailReadEvent : Event
    {
        public MailReadEvent(Guid messageId)
        {
            MessageId = messageId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
    }

    public class MessageAcknowledgedEvent : Event
    {
        public MessageAcknowledgedEvent(Guid messageId)
        {
            MessageId = messageId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
    }

    public class MessageMarkedAsReadEvent : Event
    {
        public MessageMarkedAsReadEvent(Guid messageId)
        {
            MessageId = messageId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
    }

    public class MessageMarkedAsDeletedEvent : Event
    {
        public MessageMarkedAsDeletedEvent(Guid messageId)
        {
            MessageId = messageId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
    }

    public class MessageMovedToInboxEvent : Event
    {
        public MessageMovedToInboxEvent(Guid messageId)
        {
            MessageId = messageId;
            AggregateId = messageId;
        }

        public Guid MessageId { get; private set; }
    }
}