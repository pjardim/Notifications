using EventBus.Events;
using System;

namespace Notifications.Core.ApplicationEvents
{
    public class BackOfficeCommentCreated : IntegrationEvent
    {
        public string CrewPayReportId { get; private set; }
        public string BackOfficeId { get; private set; }
        public string CrewPartyId { get; private set; }
        public string Comment { get; private set; }
        public Guid ApplicationEventId { get; private set; }
        public string ApplicationEventName { get; }

        public BackOfficeCommentCreated(string applicationEventName, string backOfficeId, string crewPartyId, string crewPayReportId, string comment)
        {
            ApplicationEventName = applicationEventName;
            BackOfficeId = backOfficeId;
            CrewPartyId = crewPartyId;
            CrewPayReportId = crewPayReportId;
            Comment = comment;
        }
    }
}