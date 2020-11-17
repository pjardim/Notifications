using EventBus.Events;
using System;

namespace Subscribing.SignalrHub.IntegrationEvents.Events
{
    public class MailCreatedIntegrationEvent : IntegrationEvent
    {
        public MailCreatedIntegrationEvent(Guid messageId, string recipientPartyId, string senderPartyIds, string subject, string body, bool requireAcknowledged)
        {
            MessageId = messageId;
            RecipientPartyId = recipientPartyId;
            SenderPartyIds = senderPartyIds;
            Subject = subject;
            Body = body;
            RequireAcknowledged = requireAcknowledged;
        }

        public Guid MessageId { get; private set; }
        public string RecipientPartyId { get; private set; }
        public string SenderPartyIds { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public bool RequireAcknowledged { get; private set; }
    }
}