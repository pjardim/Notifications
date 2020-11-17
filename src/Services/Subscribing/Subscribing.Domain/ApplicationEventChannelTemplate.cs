using Notifications.Core.SeedWork;
using System;

namespace Subscribing.Domain
{
    public class ApplicationEventChannelTemplate : Entity, IAggregateRoot
    {
        protected ApplicationEventChannelTemplate()
        {
        }

        public ApplicationEventChannelTemplate(Guid applicationEventId, Guid channelId,
           string format, string encoding, string subject, string body) : this()
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            Format = format;
            Encoding = encoding;
            Subject = subject;
            Body = body;
        }


        public Guid ApplicationEventId { get; private set; }
        public ApplicationEvent ApplicationEvent { get; private set; }
        
        public Guid ChannelId { get; private set; }
        public Channel Channel { get; private set; }

        public string Format { get; private set; }
        public string Encoding { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
    }
}