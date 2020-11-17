using Notifications.Core.Messaging;
using System;


namespace Notifying.API.Application.Commands
{
    public class StartSendingMessageCommand : Command
    {
        public StartSendingMessageCommand(Guid messageId)
        {
            MessageId = messageId;
        }

        public Guid MessageId { get; set; }
    }
}