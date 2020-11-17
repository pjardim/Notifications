using Notifications.Core.Messaging;
using Notifying.API.Model;
using System;

namespace Notifying.API.Application.Commands
{
    public class SendEmailMessageCommand : Command
    {
        private Message message;

        public Guid MessageId { get; private set; }

        public SendEmailMessageCommand(Guid messageId)
        {
            MessageId = messageId;
        }

        public SendEmailMessageCommand(Message message)
        {
            MessageId = message.Id;
            this.message = message;
        }
    }
}