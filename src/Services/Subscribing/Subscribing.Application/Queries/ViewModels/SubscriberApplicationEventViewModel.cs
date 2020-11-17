using System;
using System.Collections.Generic;
using System.Text;

namespace Subscribing.Application.Queries.ViewModels
{
    public class SubscriberApplicationEventViewModel
    {
        public Guid ApplicationEventId { get; set; }
        public ApplicationEventViewModel ApplicationEvent { get; set; }

        public string SubscriberPartyId { get; set; }
        public SubscriberViewModel Subscriber { get; set; }

        public Guid ChannelId { get; set; }
        public ChannelViewModel Channel { get; set; }
    }
}
