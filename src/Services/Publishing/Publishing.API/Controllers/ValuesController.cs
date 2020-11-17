using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notifications.Core.ApplicationEvents;
using Publishing.API.Services;
using Subscribing.Application.Queries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Publishing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICrewPayService _crewPayService;
        private readonly ILogger<ValuesController> _logger;

        public IEventBus _eventBus { get; }
        private readonly IApplicationEventQueries _applicationEventQueries;
        private readonly ISubscriberQueries _subscriberQueries;

        public ValuesController(ICrewPayService crewPayService, IEventBus eventBus,
            ILogger<ValuesController> logger,
            IApplicationEventQueries applicationEventQueries,
            ISubscriberQueries subscriberQueries)
        {
            _crewPayService = crewPayService ?? throw new ArgumentNullException(nameof(crewPayService));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationEventQueries = applicationEventQueries ?? throw new ArgumentNullException(nameof(applicationEventQueries));
            _subscriberQueries = subscriberQueries;
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
            Console.WriteLine("received a Post: " + value);
            // _messageService.Enqueue(value);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var crewSubscriber = await _subscriberQueries.GetSubscriberByNameAsync("CrewName");
            var backoffcieSubscriber = await _subscriberQueries.GetSubscriberByNameAsync("AdminName");

            var backOfficeCommentApplicationEvent = await _applicationEventQueries.GetApplicationEventByNameAsync("BackOfficeCommentCreated");
            var crewMemberCommentApplicationEvent = await _applicationEventQueries.GetApplicationEventByNameAsync("CrewCommentCreated");
            Random rnd = new Random();

            var payLoadBackoffice = new Dictionary<string, string> {
                { "BackOfficeId", backoffcieSubscriber.SubscriberPartyId },
                { "CrewPartyId", crewSubscriber.SubscriberPartyId },
                { "CrewPayReportDetails", "empNo={{}}&crewpayReportId={{}}&crewPayReportStateId={{}}&RosterPeriodIds={{}}&CrewPaySubReportState={{}}" },
                { "CrewPaySubReportState", "current" },
                { "Comment", "Hi, This is My Back office Comment and this nunber Is randon :" + rnd.Next(1,10) },
            };
            var crewCommentPayLoad = new Dictionary<string, string> {
                { "CrewPayReportDetails", "empNo={{}}&crewpayReportId={{}}&crewPayReportStateId={{}}&RosterPeriodIds={{}}&CrewPaySubReportState={{}}" },
                { "Comment", "Hi, This is My Crew Comment and this nunber Is randon :" + rnd.Next(1,10) },
            };

            var crewMemberComment = new GenericApplicationEvent(crewMemberCommentApplicationEvent.ApplicationEventName, crewSubscriber.SubscriberPartyId, crewCommentPayLoad);

            var backofficeComment = new GenericApplicationEvent(backOfficeCommentApplicationEvent.ApplicationEventName, backoffcieSubscriber.SubscriberPartyId, payLoadBackoffice);
            // _eventBus.Publish(genericApplicationevent);

            _eventBus.Publish(crewMemberComment);
            _eventBus.Publish(backofficeComment);

            return Ok();
        }
    }

    public class GenericEvent : IntegrationEvent
    {
        public Dictionary<string, string> KeyValue { get; set; }
    }
}