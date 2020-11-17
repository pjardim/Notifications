using Subscribing.Application.Queries.ViewModels;
using System;
using System.Collections.Generic;

namespace WebMVC.ViewModels
{
    public class ApplicationEventChannelCreateViewModel
    {
        public ApplicationEventChannelCreateViewModel()
        {
            ApplicationEventChannelViewModels = new List<ApplicationEventChannelViewModel>();
        }

        public Guid ApplicationEventId { get; set; }
        public ApplicationEventViewModel ApplicationEvent { get; set; }

        public List<ApplicationEventChannelViewModel> ApplicationEventChannelViewModels { get; set; }
    }
}