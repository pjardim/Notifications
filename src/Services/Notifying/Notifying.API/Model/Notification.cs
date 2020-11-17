using MongoDB.Bson.Serialization.Attributes;
using Notifications.Core.SeedWork;
using System;
using System.Collections.Generic;

namespace Notifying.API.Model
{
    [BsonIgnoreExtraElements]
    public class Notification : IAggregateRoot
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonRequired]
        public Guid MessageId { get; private set; }

        [BsonRequired]
        public string PublisherPartyId { get; private set; }

        [BsonRequired]
        public Guid ApplicationEventId { get; private set; }

        [BsonRequired]
        public string ApplicationEventName { get; private set; }

        [BsonRequired]
        public int DelaySendMinutes { get; private set; }

        [BsonRequired]
        public string PayLoad { get; private set; }

        [BsonRequired]
        public DateTime CreatedDate { get; private set; }

        [BsonRequired]
        public List<string> NotificationSubscribersPartyIds { get; private set; }

        private Notification()
        {
            NotificationSubscribersPartyIds = new List<string>();
        }

        public Notification(Guid id, Guid messageId, string publisherPartyId, Guid applicationEventId,
            string applicationEventName, int delaySendMinutes, string payLoad) : this()
        {
            Id = id;
            MessageId = messageId;
            PublisherPartyId = publisherPartyId;
            ApplicationEventId = applicationEventId;
            ApplicationEventName = applicationEventName;
            DelaySendMinutes = delaySendMinutes;
            PayLoad = payLoad;
            CreatedDate = DateTime.Now;
        }

        internal void RelateMessage(Guid messageId)
        {
            MessageId = messageId;
        }

        public void AddNotificationSubscribers(List<string> partyIds)
        {
            foreach (var partyId in partyIds)
            {
                NotificationSubscribersPartyIds.Add(partyId);
            }
        }
    }
}