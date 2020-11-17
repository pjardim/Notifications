using MediatR;
using System;

namespace Notifications.Core.Messaging
{
    public abstract class Event : DomainMessage, INotification
    {
        public DateTime Timestamp { get; private set; }

        protected Event()
        {
            Timestamp = DateTime.Now;
        }
    }
}