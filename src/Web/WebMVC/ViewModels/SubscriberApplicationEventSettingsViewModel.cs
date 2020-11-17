using Microsoft.AspNetCore.Mvc.Rendering;
using Subscribing.Application.Queries.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebMVC.ViewModels
{
    public class SubscriberApplicationEventSettingsViewModel
    {
        public SubscriberApplicationEventSettingsViewModel()
        {
            SubscriberApplicationEvents = new List<SubscriberApplicationEventViewModel>();
        }

        public string SubscriberPartyId { get; set; }
        public SubscriberViewModel Subscriber { get; set; }

        public List<SubscriberApplicationEventViewModel> SubscriberApplicationEvents { get; set; }

        [DisplayName("Subscribers")]
        public IEnumerable<SelectListItem> ApplicationEvents { get; set; }

        [DisplayName("Channels")]
        public IEnumerable<SelectListItem> ChannelsList { get; set; }
    }
}