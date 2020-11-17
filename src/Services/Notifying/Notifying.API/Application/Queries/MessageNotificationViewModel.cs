using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifying.API.Application.Queries
{
    public class MessageWithNotification
    {
        public string PartyId { get; private set; }
        public Guid ApplicationEventId { get; private set; }
        public string ApplicationEventName { get; private set; }
        public string Subject { get; private set; }
        public string Comment { get; private set; }
    }
}
