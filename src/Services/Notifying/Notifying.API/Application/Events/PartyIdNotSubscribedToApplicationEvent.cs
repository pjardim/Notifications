using Notifications.Core.Messaging;
using Notifying.API.Model;

namespace Notifying.API.Application.Events
{
    public class PartyIdNotSubscribedToApplicationEvent : Event
    {
        public PartyIdNotSubscribedToApplicationEvent(Notification messageNotification)
        {
            MessageNotification = messageNotification;
        }

        public Notification MessageNotification { get; }
    }
}