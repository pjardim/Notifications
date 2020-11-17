using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifying.API.Application.IntegrationEvents.Events
{
    public class MessageStatusChangedToDelayTimeExpiredIntegrationEvent : IntegrationEvent
    {
        public MessageStatusChangedToDelayTimeExpiredIntegrationEvent(Guid messageId)
        {
            MessageId = messageId;
        }

        public Guid MessageId { get; }
    }
}
