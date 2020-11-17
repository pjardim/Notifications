using Notifications.Core.Messaging;
using Notifying.API.Model;
using System;

namespace Notifying.API.Application.Commands
{
    public class AddNotificationToMessageCommand : Command
    {
        public Guid MessageId { get; private set; }

        public Notification MessageNotification { get; private set; }

        public AddNotificationToMessageCommand()
        {
        }

        public AddNotificationToMessageCommand(Guid messageId, Notification messageNotification) : this()
        {
            MessageId = messageId;
            MessageNotification = messageNotification;
        }
    }
}