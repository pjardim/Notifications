using Notifications.Core.Messaging;
using Notifying.API.Model;
using Subscribing.Application.Queries.ViewModels;
using System;

namespace Notifying.API.Application.Commands
{
    public class SendNotificationsAppMessageCommand : Command
    {
        public SendNotificationsAppMessageCommand(Guid messageId)
        {
            MessageId = messageId;
        }

        public SendNotificationsAppMessageCommand(Message message, ApplicationEventChannelTemplateViewModel template)
        {
            MessageId = message.Id;
            Message = message;
            Template = template;
            AggregateId = MessageId;
        }

        public Guid MessageId { get; private set; }
        public Message Message { get; private set; }
        public ApplicationEventChannelTemplateViewModel Template { get; private set; }
    }
}