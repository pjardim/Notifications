using Notifications.Core.SeedWork;
using System;

namespace Subscribing.Domain
{
    public class ApplicationEventChannel : Entity, IAggregateRoot
    {
        protected ApplicationEventChannel()
        {
        }

        public ApplicationEventChannel(Guid applicationEventId, Guid channelId, int delayedSendMinutes, bool enabled, bool requireAcknowledgement) : this()
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            DelayedSendMinutes = delayedSendMinutes;
            Enabled = enabled;
            RequireAcknowledgement = requireAcknowledgement;
        }

        public Guid ApplicationEventId { get; private set; }
        public ApplicationEvent ApplicationEvent { get; private set; }

        public Guid ChannelId { get; private set; }
        public Channel Channel { get; private set; }
        public int DelayedSendMinutes { get; private set; }
        public bool Enabled { get; private set; }
        public bool RequireAcknowledgement { get; private set; }
    }
}