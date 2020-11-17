using System;
using System.Collections.Generic;

namespace Subscribing.Application.Queries.ViewModels
{
    public class MailBoxItemViewModel
    {
        public Guid MessageId { get; set; }
        public string SenderPartyIds { get; set; }

        public IEnumerable<SubscriberViewModel> Senders { get; set; }
        public string RecipientPartyId { get; set; }
        public SubscriberViewModel Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public bool Read { get; set; }
        public bool RequireAcknowledged { get; set; }
        public bool Acknowledged { get; set; }
        public bool Deleted { get; set; }
        public bool Excluded { get; set; }
        public DateTime CreatedDate { get; set; }

     
    }
}