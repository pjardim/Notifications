using Subscribing.Application.Queries.ViewModels;
using System;
using System.Collections.Generic;

namespace WebMVC.ViewModels
{
    public class ApplicationEventParameterIndexViewModel
    {
        public ApplicationEventParameterIndexViewModel()
        {
            ApplicationEventParameters = new List<ApplicationEventParameterViewModel>();
        }

        public Guid ApplicationEventId { get; set; }
        public ApplicationEventViewModel ApplicationEvent { get; set; }
        public IEnumerable<ApplicationEventParameterViewModel> ApplicationEventParameters { get; set; }
    }
}