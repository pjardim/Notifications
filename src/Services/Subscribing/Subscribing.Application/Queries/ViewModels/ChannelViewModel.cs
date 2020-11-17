using System;
using System.Collections.Generic;

namespace Subscribing.Application.Queries.ViewModels
{
    public class ChannelViewModel
    {
        public Guid Id { get; set; }
        public string ChannelName { get; set; }

        public ICollection<SubscriberChannelViewModel> SubscriberChannels { get; set; }

        public ICollection<ApplicationEventChannelViewModel> ApplicationEventChannels { get; set; }
    }
}