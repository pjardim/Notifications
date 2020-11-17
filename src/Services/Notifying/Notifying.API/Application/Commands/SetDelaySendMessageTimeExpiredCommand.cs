using Notifications.Core.Messaging;
using System;

namespace Notifying.API.Application.Commands
{
    public class SetDelaySendMessageTimeExpiredCommand : Command
    {
        public SetDelaySendMessageTimeExpiredCommand(Guid messageId)
        {
            MessageId = messageId;
        }

        public Guid MessageId { get; set; }
    }
}