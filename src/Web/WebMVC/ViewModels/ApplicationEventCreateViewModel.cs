using Microsoft.AspNetCore.Mvc.Rendering;
using Subscribing.Application.Queries.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.ViewModels
{
    public class ApplicationEventCreateViewModel
    {
        public ApplicationEventCreateViewModel()
        {
            SubscriberFilters = new List<SubscriberFilterViewModel>();
        }

        [Required]
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Application Event Name")]
        public string ApplicationEventName { get; set; }

        public string Description { get; set; }

        [DisplayName("Subscriber Filters")]
        public List<SubscriberFilterViewModel> SubscriberFilters { get; set; }

        [DisplayName("Subscriber Filters")]
        public IEnumerable<SelectListItem> SubscriberFiltersList { get; set; }

        public IEnumerable<ApplicationEventParameterViewModel> ApplicationEventParameters { get; set; }

        public IEnumerable<SubscriberApplicationEventViewModel> SubscriberApplicationEvents { get; set; }

        public IEnumerable<ApplicationEventChannelViewModel> ApplicationEventChannels { get; set; }
    }
}