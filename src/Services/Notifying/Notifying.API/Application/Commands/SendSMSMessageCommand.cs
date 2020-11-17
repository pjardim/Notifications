using Notifications.Core.Messaging;
using Notifying.API.Model;
using System;

namespace Notifying.API.Application.Commands
{
    public class SendSMSMessageCommand : Command
    {
        public Message Message { get; private set; }

        public SendSMSMessageCommand(Message message)
        {
            Message = message;
        }

     
    }
}