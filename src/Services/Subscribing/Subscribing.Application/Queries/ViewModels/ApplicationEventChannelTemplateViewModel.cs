using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Subscribing.Application.Queries.ViewModels
{
    public class ApplicationEventChannelTemplateViewModel
    {
        public Guid ApplicationEventId { get; set; }
        public ApplicationEventViewModel ApplicationEvent { get; set; }

        public Guid ChannelId { get; set; }
        public ChannelViewModel Channel { get; set; }

        public string Format { get; set; }
        public string Encoding { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        [DisplayName("Channels")]
        public IEnumerable<SelectListItem> ChannelsList { get; set; }

       
    }
}