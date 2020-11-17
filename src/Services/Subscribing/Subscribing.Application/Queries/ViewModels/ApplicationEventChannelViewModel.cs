using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Subscribing.Application.Queries.ViewModels
{
    public class ApplicationEventChannelViewModel
    {
        public Guid ApplicationEventId { get; set; }
        public ApplicationEventViewModel ApplicationEvent { get; set; }

        public Guid ChannelId { get; set; }
        public ChannelViewModel Channel { get; set; }
        public int DelayedSendMinutes { get; set; }
        public bool Enabled { get; set; }
        public bool RequireAcknowledgement { get; set; }

        [DisplayName("Channels")]
        public IEnumerable<SelectListItem> ChannelsList { get; set; }
    }
}