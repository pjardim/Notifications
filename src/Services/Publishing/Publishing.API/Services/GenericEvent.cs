using EventBus.Events;

namespace Publishing.API.Services
{
    internal class GenericEvent : IntegrationEvent
    {
        public string PartyId { get; set; }
        public string CrewComent { get; set; }
        public string EventType { get; set; }

        public GenericEvent(string eventType, string partyId, string crewComent)
        {
            EventType = eventType;
            PartyId = partyId;
            CrewComent = crewComent;
        }
    }
}