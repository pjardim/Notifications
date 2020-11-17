using Notifications.Core.ApplicationEvents;
using Notifications.Core.Messaging;

namespace Notifying.API.Application.Commands
{
    public class StartGenericEventProcessCommand : Command
    {
        public StartGenericEventProcessCommand(GenericApplicationEvent applicationEvent)
        {
            ApplicationEvent = applicationEvent;
            
        }

        public GenericApplicationEvent ApplicationEvent { get; set; }
    }
}