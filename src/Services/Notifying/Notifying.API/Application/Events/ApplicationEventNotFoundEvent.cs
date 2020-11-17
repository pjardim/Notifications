using Notifications.Core.ApplicationEvents;
using Notifications.Core.Messaging;

namespace Notifying.API.Application.Events
{
    public class ApplicationEventNotFoundEvent : Event
    {
        public string ApplicationEventName { get; private set; }

        public ApplicationEventNotFoundEvent(string applicationEventName, GenericApplicationEvent applicationEvent)
        {
            ApplicationEventName = applicationEventName;
        }
    }
}