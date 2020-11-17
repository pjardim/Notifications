using Notifications.Core.Messaging;
using System;
using System.Collections.Generic;

namespace Notifying.API.Application.Commands
{
    public class CreateNewMessageCommand : Command
    {
        public CreateNewMessageCommand(Guid id, string publihserPatyId,
            Guid applicationEventId, List<string> recipientsParties,
            string applicationEventName, int delaySendMinutes, string payLoad)
        {
            Id = id;
            PublihserPatyId = publihserPatyId;
            ApplicationEventId = applicationEventId;
            RecipientsParties = recipientsParties;
            ApplicationEventName = applicationEventName;
            DelaySendMinutes = delaySendMinutes;
            PayLoad = payLoad;
        }

        public Guid Id { get; }
        public string PublihserPatyId { get; private set; }
        public Guid ApplicationEventId { get; }
        public List<string> RecipientsParties { get; }
        public string ApplicationEventName { get; }
        public int DelaySendMinutes { get; }
        public string PayLoad { get; }
    }
}