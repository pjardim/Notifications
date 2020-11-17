using System;

namespace Subscribing.Application.Queries.ViewModels
{
    public class SubscriberChannelViewModel
    {
        public Guid ApplicationEventId { get; set; }
        public ApplicationEventViewModel ApplicationEvent { get; set; }

        public string SubscriberPartyId { get; set; }
        public SubscriberViewModel Subscriber { get; set; }
    }
}