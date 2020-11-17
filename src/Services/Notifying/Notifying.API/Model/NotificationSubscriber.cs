using Notifications.Core.SeedWork;
using System;

namespace Notifying.API.Model
{
    public class NotificationSubscriber : Entity, IAggregateRoot
    {
        public NotificationSubscriber(Guid messageNotificationId, string partyId)
        {
            MessageNotificationId = messageNotificationId;
            PartyId = partyId;
        }

        public Guid MessageNotificationId { get; set; }

        public string PartyId { get; set; }
    }
}