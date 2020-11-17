using EventBus.Events;

namespace Notifications.Core.ApplicationEvents
{
    public class CommentCreated : IntegrationEvent
    {
        public string CrewPayReportId { get; private set; }
        public string Comment { get; private set; }
        public string PublisherPartyId { get; private set; }
        public string RecipientPartyId { get; private set; }

        public string ApplicationEventName { get; private set; }
        public string Subject { get; private set; }

        public CommentCreated(string applicationEventName, string publisherPartyId, string recipientPartyId, string crewPayReportId, string comment, string subject)
        {
            ApplicationEventName = applicationEventName;
            PublisherPartyId = publisherPartyId;
            RecipientPartyId = recipientPartyId;
            CrewPayReportId = crewPayReportId;
            Comment = comment;
            Subject = subject;
        }
    }
}