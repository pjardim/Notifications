using Microsoft.AspNetCore.Mvc.Rendering;
using Subscribing.Application.Queries.ViewModels;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebMVC.ViewModels
{
    public class SubscriberGroupSettingsViewModel
    {
        public string SubscriberPartyId { get; set; }
        public SubscriberViewModel Subscriber { get; set; }

        public List<int> SelectedGroups { get; set; }

        [DisplayName("Subscriber Groups")]
        public IEnumerable<SelectListItem> SubscriberGroups { get; set; }

        [DisplayName("Subscribers")]
        public IEnumerable<SelectListItem> SubscribersList { get; set; }
    }
}