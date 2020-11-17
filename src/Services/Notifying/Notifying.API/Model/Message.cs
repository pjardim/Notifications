using MongoDB.Bson.Serialization.Attributes;
using Notifications.Core.SeedWork;
using Notifying.API.Application.DomainEvents;
using System;
using System.Collections.Generic;

namespace Notifying.API.Model
{
    [BsonIgnoreExtraElements]
    public class Message : NoSqlEntity, IAggregateRoot
    {
        internal Message()
        {
            Notifications = new List<Notification>();
        }

        public Message(Guid id, string messageChannel, bool requireAcknowledgement) : this()
        {
            Id = id;
            MessageChannel = messageChannel;
            RequireAcknowledgement = requireAcknowledgement;
            SetPendingStatus();
            AddMessageStartedDomainEvent(this.Id, MessageChannel);
            CreatedDate = DateTime.Now;
        }

        private void AddMessageStartedDomainEvent(Guid messageId, string messageChannel)
        {
            var messageStartedDomainEvent = new MessageStartedDomainEvent(messageId, messageChannel);
            AddDomainEvent(messageStartedDomainEvent);
        }

        [BsonId]
        public Guid Id { get; set; }

        [BsonRequired]
        public string MessageChannel { get; private set; }

        [BsonRequired]
        public bool RequireAcknowledgement { get; private set; }

        [BsonRequired]
        public DateTime CreatedDate { get; private set; }


        [BsonRequired]
        [BsonElement("MessageStatusId")]
        private int _messageStatusId;


        [BsonIgnore]
        public MessageStatus MessageStatus { get; private set; }

        [BsonRequired]
        public ICollection<Notification> Notifications { get; private set; }

        public void SetPendingStatus()
        {
            _messageStatusId = MessageStatus.Pending.Id;
        }

        // DDD Patterns comment
        // This Message AggregateRoot's method "AddMessageNotification()" should be the only way to add Notificiations to the Message,
        // so any behavior  and validations are controlled by the AggregateRoot
        // in order to maintain consistency between the whole Aggregate.
        public void AddMessageNotification(Notification messageNotification)
        {
            messageNotification.RelateMessage(Id);
            Notifications.Add(messageNotification);
            this.SetPendingStatus();

            AddDomainEvent(new NotificationAddedToMessageDomainEvent(messageNotification.Id, messageNotification.MessageId, messageNotification.PublisherPartyId,
                messageNotification.ApplicationEventId, messageNotification.ApplicationEventName, messageNotification.DelaySendMinutes, messageNotification.PayLoad));
        }

        public void SetDelayMessageTimeExpiredStatus()
        {
            if (_messageStatusId != MessageStatus.Canceled.Id)
            {
                AddDomainEvent(new MessageStatusChangedToDelayTimeExpiredDomainEvent(Id));
                _messageStatusId = MessageStatus.DelayTimeExpired.Id;
            }
        }
    }
}