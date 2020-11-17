using Notifications.Core.SeedWork;
using System.Collections.Generic;

namespace Subscribing.Domain
{
    public class Channel : Entity, IAggregateRoot
    {
        protected Channel()
        {
            ApplicationEventChannels = new List<ApplicationEventChannel>();
        }

        public Channel(string channelName) : this()
        {
            ChannelName = channelName;
        }

        public string ChannelName { get; private set; }

        public ICollection<ApplicationEventChannel> ApplicationEventChannels { get; private set; }

        public ICollection<ApplicationEventChannelTemplate> ApplicationEventChannelTemplates { get; private set; }
    }
}