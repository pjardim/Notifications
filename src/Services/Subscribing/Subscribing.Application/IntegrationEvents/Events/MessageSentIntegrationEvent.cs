using EventBus.Events;
using System;
using System.Collections.Generic;

namespace Subscribing.Application.IntegrationEvents.Events
{
    public class MessageSentIntegrationEvent : IntegrationEvent
    {
        public MessageSentIntegrationEvent(Guid messageId, string sendersPartyIds, string recipientPartyId, string subject, string body, bool requireAcknowledgement)
        {
            MessageId = messageId;
            SendersPartyIds = sendersPartyIds;
            RecipientPartyId = recipientPartyId;
            Subject = subject;
            Body = body;
            RequireAcknowledgement = requireAcknowledgement;
            SentDate = DateTime.Now;
        }

        public Guid MessageId { get; }
        public string SendersPartyIds { get; private set; }
        public string RecipientPartyId { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public bool RequireAcknowledgement { get; private set; }
        public DateTime SentDate { get; private set; }
    }
}