using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Subscribing.Application.Queries.ViewModels
{
    public class SubscriberFilterViewModel 
    {
        public Guid Id { get; set; }

        [DisplayName("Filter Type")]
        public string FilterType { get;  set; }

        [DisplayName("Filter Value")]
        public string FilterValue { get;  set; }

        public Guid ApplicationEventId { get;  set; }
        public ApplicationEventViewModel ApplicationEvent { get;  set; }
    }
}
