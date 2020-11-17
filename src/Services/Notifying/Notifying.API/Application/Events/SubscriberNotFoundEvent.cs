using Notifications.Core.Messaging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notifying.API.Application.Events
{
    public class SubscriberNotFoundEvent : Event
    {
        public SubscriberNotFoundEvent(string recipientPartyId)
        {
            RecipientPartyId = recipientPartyId;
        }

        public string RecipientPartyId { get; }
    }
}
