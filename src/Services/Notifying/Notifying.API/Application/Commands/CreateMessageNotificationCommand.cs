using Notifications.Core.Messaging;
using Notifying.API.Model;

namespace Notifying.API.Application.Commands
{
    public class CreateMessageNotificationCommand : Command
    {
        public CreateMessageNotificationCommand(string publisherPartyId, string channel, bool requireAcknowledgement,
            Notification messageNotification)
        {
            PublihserPatyId = publisherPartyId;
            Channel = channel;
            RequireAcknowledgement = requireAcknowledgement;
            MessageNotification = messageNotification;
        }

        public string PublihserPatyId { get; private set; }
        public string Channel { get; private set; }
        public bool RequireAcknowledgement { get; private set; }
        public Notification MessageNotification { get; private set; }
    }
}