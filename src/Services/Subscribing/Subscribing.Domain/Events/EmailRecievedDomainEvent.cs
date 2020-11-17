using Notifications.Core.Messaging.CommonMessages.DomainEvents;
using System;

namespace Subscribing.Domain.Events
{
    public class EmailRecievedDomainEvent : DomainEvent
    {
        public EmailRecievedDomainEvent(MailBoxItem mailBoxItem, Guid messageId, string recipientPartyId,
            string senderPartyIds, string subject, string body, bool requireAcknowledged) : base(messageId)
        {
            MailBoxItem = mailBoxItem;
            MessageId = messageId;
            RecipientPartyId = recipientPartyId;
            SenderPartyIds = senderPartyIds;
            Subject = subject;
            Body = body;
            RequireAcknowledged = requireAcknowledged;
        }

        public MailBoxItem MailBoxItem { get; set; }
        public Guid MessageId { get; private set; }
        public string RecipientPartyId { get; private set; }
        public string SenderPartyIds { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public bool RequireAcknowledged { get; private set; }
    }
}