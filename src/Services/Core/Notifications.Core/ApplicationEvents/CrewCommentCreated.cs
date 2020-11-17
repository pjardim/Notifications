using EventBus.Events;
using Notifications.Core.Messaging;
using System;

namespace Notifications.Core.ApplicationEvents
{
    public class CrewCommentCreated : IntegrationEvent
    {
        public string PublisherPartyId { get; private set; }
        public string Comment { get; private set; }
        public string ApplicationEventName { get; }
        public Guid ApplicationEventId { get; private set; }
        public string CrewPayReportId { get; private set; }

        public CrewCommentCreated(string publisherPartyId, string comment, string applicationEventName, Guid applicationEventId, string crewPayReportId)
        {
            PublisherPartyId = publisherPartyId;
            Comment = comment;
            ApplicationEventName = applicationEventName;
            ApplicationEventId = applicationEventId;
            CrewPayReportId = crewPayReportId;
        }
    }
}