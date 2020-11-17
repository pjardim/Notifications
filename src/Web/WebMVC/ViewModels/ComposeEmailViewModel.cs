using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel;

namespace WebMVC.ViewModels
{
    public class ComposeEmailViewModel
    {
        public ComposeEmailViewModel()
        {
            SubscribersList = new List<SelectListItem>();
        }

        public string SenderPartyId { get; set; }
        public List<string> RecipientPartyIds { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool RequireAcknowledgement { get; set; }


        [DisplayName("Subscribers")]
        public IEnumerable<SelectListItem> SubscribersList { get; set; }
    }
}