
using EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System;

namespace Publishing.API.Services
{
    public class CrewPayService : ICrewPayService
    {
        private readonly ILogger<CrewPayService> _logger;
        private readonly IEventBus _eventBus;

        public CrewPayService(IEventBus eventBus, ILogger<CrewPayService> logger)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void PublishCrewCreatedCommentEvent(string crewId, string crewComent)
        {
        }

        public bool SaveCrewComment(string crewId, string crewComent)
        {
            //Save...

            //Publish Event
            PublishCrewCreatedCommentEvent(crewId, crewComent);
            return true;
        }
    }
}