using EventBus.Events;
using System.Collections.Generic;

namespace Notifications.Core.ApplicationEvents
{
    public class GenericApplicationEvent : IntegrationEvent
    {
        public GenericApplicationEvent(string applicationEventType, string publisherPartyId, Dictionary<string, string> payload)
        {
            Name = applicationEventType;
            PublisherPartyId = publisherPartyId;

            Payload = payload;
        }

        public string Name { get; set; }
        public string PublisherPartyId { get; set; }

        public Dictionary<string, string> Payload { get; set; }
    }
}