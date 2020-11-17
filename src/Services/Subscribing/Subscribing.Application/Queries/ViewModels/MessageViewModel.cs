using System;
using System.Collections.Generic;

namespace Subscribing.Application.Queries.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }

        public string MessageChannel { get;  set; }

        public DateTime CreatedDate { get;  set; }

        public int MessageStatusId { get;  set; }

        public ICollection<NotificationViewModel> Notifications { get;  set; }
    }

    public class NotificationViewModel
    {
        public Guid Id { get; set; }

        public Guid MessageId { get;  set; }

        public string PublisherPartyId { get;  set; }

        public Guid ApplicationEventId { get;  set; }

        public string ApplicationEventName { get;  set; }

        public int DelaySendMinutes { get;  set; }

        public string PayLoad { get;  set; }

        public DateTime CreatedDate { get;  set; }
    }
}