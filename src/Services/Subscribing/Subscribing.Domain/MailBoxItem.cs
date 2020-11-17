using Notifications.Core.SeedWork;
using Subscribing.Domain.Events;
using System;

namespace Subscribing.Domain
{
    public class MailBoxItem : Entity, IAggregateRoot
    {
        protected MailBoxItem()
        {
        }

        public MailBoxItem(Guid messageId, string recipientPartyId, string publishersPartyIds, string subject,
            string body, bool requireAcknowledged)
        {
            MessageId = messageId;
            RecipientPartyId = recipientPartyId;
            SenderPartyIds = publishersPartyIds;
            Subject = subject;
            Body = body;
            RequireAcknowledged = requireAcknowledged;
            AddNewEmailRecievedDomainEvent(messageId, recipientPartyId, publishersPartyIds, subject, body, requireAcknowledged);
        }

        private void AddNewEmailRecievedDomainEvent(Guid messageId, string recipientPartyId, string senderPartyIds, string subject, string body, bool requireAcknowledged)
        {
            var emailRecievedDomainEvent = new EmailRecievedDomainEvent(this, messageId, recipientPartyId, senderPartyIds,
                 subject, body, requireAcknowledged);

            this.AddDomainEvent(emailRecievedDomainEvent);
        }

        public Guid MessageId { get; private set; }
        public string RecipientPartyId { get; private set; }
        public Subscriber Recipient { get; set; }

        public string SenderPartyIds { get; private set; }
        public bool DirectEmail { get; private set; }

        public string Subject { get; private set; }
        public string Body { get; private set; }

        public bool Read { get; private set; }

        public bool RequireAcknowledged { get; private set; }
        public bool Acknowledged { get; private set; }
        public bool Deleted { get; private set; }
        public bool Excluded { get; private set; }
        public DateTime CreatedDate { get; private set; }

        public void SetDeletedMail()
        {
            Deleted = true;
        }

        public void SetExcludeEmail()
        {
            Excluded = true;
        }

        public void SetReadEmail()
        {
            Read = true;
        }

        public void MoveToInbox()
        {
            Excluded = false;
            Deleted = false;
        }

        public void SetAcknowledged()
        {
            Acknowledged = true;
        }

        public void SetDirectEmail()
        {
            DirectEmail = true;
        }
    }
}