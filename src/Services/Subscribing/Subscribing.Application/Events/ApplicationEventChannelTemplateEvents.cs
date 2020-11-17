using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Events
{
    internal class ApplicationEventChannelTemplateCreatedEvent : Event
    {
        public ApplicationEventChannelTemplateCreatedEvent(Guid applicationEventId, Guid channelId, string format, string encoding, string subject, string body)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            Format = format;
            Encoding = encoding;
            Subject = subject;
            Body = body;
            AggregateId = applicationEventId;
        }

        public Guid ApplicationEventId { get; private set; }
        public Guid ChannelId { get; private set; }
        public string Format { get; private set; }
        public string Encoding { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
    }

    public class ApplicationEventChannelTemplateUpdatedEvent : Event
    {
        public ApplicationEventChannelTemplateUpdatedEvent(Guid applicationEventId, Guid channelId, string format, string encoding, string subject, string body)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            Format = format;
            Encoding = encoding;
            Subject = subject;
            Body = body;
            AggregateId = applicationEventId;
        }

        public Guid ApplicationEventId { get; private set; }
        public Guid ChannelId { get; private set; }
        public string Format { get; private set; }
        public string Encoding { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
    }
}