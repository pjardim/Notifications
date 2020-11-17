using Notifications.Core.Messaging;
using System;
using System.Collections.Generic;

namespace Notifying.API.Application.Commands
{
    public class GroupMessageNotificationCommand : Command
    {
        public GroupMessageNotificationCommand(Guid messageId, Guid applicationEventId, List<string> recipientPartyIds,
            string applicationEventName, string publisherPartyId, int delaySendMinutes, string payLoad)
        {
            MessageId = messageId;
            ApplicationEventId = applicationEventId;
            RecipientPartyIds = recipientPartyIds;
            ApplicationEventName = applicationEventName;
            PublisherPartyId = publisherPartyId;
            DelaySendMinutes = delaySendMinutes;
            PayLoad = payLoad;
        }

        public Guid MessageId { get; private set; }
        public Guid ApplicationEventId { get; private set; }
        public List<string> RecipientPartyIds { get; private set; }
        public string ApplicationEventName { get; private set; }
        public string PublisherPartyId { get; private set; }
        public int DelaySendMinutes { get; private set; }
        public string PayLoad { get; private set; }
    }
}