using System;

namespace Notifications.Core.Messaging
{
    public abstract class DomainMessage
    {
        public string DomainMessageType { get; protected set; }
        public Guid AggregateId { get; set; } // MessageId

        protected DomainMessage()
        {
            DomainMessageType = GetType().Name;
        }
    }
}