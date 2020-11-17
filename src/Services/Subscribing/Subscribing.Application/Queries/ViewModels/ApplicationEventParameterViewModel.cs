using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Subscribing.Application.Queries.ViewModels
{
    public class ApplicationEventParameterViewModel
    {
        public Guid Id { get; set; }
        public Guid ApplicationEventId { get; set; }
        public ApplicationEventViewModel ApplicationEvent { get; set; }

        [DisplayName("Application Events")]
        public string ParameterName { get; set; }
        public string Description { get; set; }

        [DisplayName("Application Events")]
        public IEnumerable<SelectListItem> ApplicationEventList { get; set; }
    }
}