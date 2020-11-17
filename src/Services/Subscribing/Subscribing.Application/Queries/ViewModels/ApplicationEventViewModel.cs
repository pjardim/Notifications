using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Subscribing.Application.Queries.ViewModels
{
    public class ApplicationEventViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [DisplayName("Application Event Name")]
        public string ApplicationEventName { get; set; }

        public string Description { get; set; }

        [DisplayName("Subscriber Filters")]
        public IEnumerable<SubscriberFilterViewModel> SubscriberFilters { get; set; }

        [DisplayName("Subscriber Filters")]
        public IEnumerable<SelectListItem> SubscriberFiltersList { get; set; }

        public IEnumerable<ApplicationEventParameterViewModel> ApplicationEventParameters { get; set; }

        public IEnumerable<SubscriberApplicationEventViewModel> SubscriberApplicationEvents { get; set; }

        public IEnumerable<ApplicationEventChannelViewModel> ApplicationEventChannels { get; set; }
    }
}