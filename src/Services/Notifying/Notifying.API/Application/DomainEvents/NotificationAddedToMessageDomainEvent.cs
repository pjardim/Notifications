using Notifications.Core.Messaging.CommonMessages.DomainEvents;
using Notifying.API.Model;
using System;
using System.Collections.Generic;

namespace Notifying.API.Application.DomainEvents
{
    public class NotificationAddedToMessageDomainEvent : DomainEvent
    {
        public Guid NotificationId { get; set; }
        public Guid MessageId { get; set; }
        public string PartyId { get; private set; }
        public string ReceipentPartyId { get; private set; }
        public Guid ApplicationEventId { get; private set; }
        public string ApplicationEventName { get; private set; }
        public int DelaySendMinutes { get; private set; }
        public string PayLoad { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public IEnumerable<NotificationSubscriber> NotificationSubscribers { get; private set; }

        public NotificationAddedToMessageDomainEvent(Guid notificationId, Guid messageId, string partyId,
            Guid applicationEventId, string applicationEventName, int delaySendMinutes, string payLoad) : base(messageId)
        {
            NotificationId = notificationId;
            MessageId = messageId;
            PartyId = partyId;

            ApplicationEventId = applicationEventId;
            ApplicationEventName = applicationEventName;
            DelaySendMinutes = delaySendMinutes;
            PayLoad = payLoad;
        }
    }
}