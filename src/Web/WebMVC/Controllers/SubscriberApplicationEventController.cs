using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Notifications.Core.Mediator;
using Notifications.Core.Notifications;
using Subscribing.Application.Commands;
using Subscribing.Application.Queries;
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Resources;
using WebMVC.Resources.Extensions;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    public class SubscriberApplicationEventController : ControllerBase
    {
        private readonly ISubscriberQueries _subscriberQueries;
        private readonly IApplicationEventQueries _applicationEventQueries;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;
        private readonly IMapper _mapper;

        public SubscriberApplicationEventController(
            INotificationHandler<DomainNotification> notifications,
            IMapper mapper,
            IApplicationEventQueries applicationEventQueries,
            ISubscriberQueries subscriberQueries,
            IMediatorHandler mediatorHandler,
            IIdentityParser<ApplicationUser> appUserParser,

            ISubscriberRepository subscriberRepository)
            : base(notifications, mediatorHandler)
        {
            _subscriberQueries = subscriberQueries ?? throw new ArgumentNullException(nameof(subscriberQueries));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _appUserParser = appUserParser ?? throw new ArgumentNullException(nameof(appUserParser));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _applicationEventQueries = applicationEventQueries ?? throw new ArgumentNullException(nameof(applicationEventQueries));
        }

        public async Task<IActionResult> Index()
        {
            var user = _appUserParser.Parse(HttpContext.User);

            var subscriber = await _subscriberQueries.GetSubscriberByPartyIdAsync(user.PartyId);


            var allChannels = await _subscriberQueries.GetAllChannelsAsync();
            var defaultChannel = allChannels.Where(x => x.ChannelName == "NotificationsApp").FirstOrDefault();

            var subscriberApplicationEventSettings = new SubscriberApplicationEventSettingsViewModel();

            subscriberApplicationEventSettings.ChannelsList = new SelectList(allChannels, "Id", "ChannelName");
            subscriberApplicationEventSettings.Subscriber = subscriber;

            var avaliableApplicationEvents = await _applicationEventQueries.GetAllApplicationEventsAsync();

            //Foreach Avaliable Application Event get the configuration or generate a default value
            foreach (var applicationEvent in avaliableApplicationEvents)
            {
                var subscriberApplicationEvent = _mapper.Map<SubscriberApplicationEventViewModel>(await _subscriberQueries.GetSubscribersApplicationEvent(user.PartyId, applicationEvent.Id));

                if (subscriberApplicationEvent == null)
                    subscriberApplicationEvent = new SubscriberApplicationEventViewModel()
                    { Subscriber = subscriber, Channel = defaultChannel, ApplicationEvent = applicationEvent, ApplicationEventId = applicationEvent.Id, ChannelId = defaultChannel.Id, SubscriberPartyId = subscriber.SubscriberPartyId }; // Create Default

                subscriberApplicationEventSettings.SubscriberApplicationEvents.Add(subscriberApplicationEvent);
            }

            return View(subscriberApplicationEventSettings);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SubscriberApplicationEventSettingsViewModel model)
        {
            foreach (var item in model.SubscriberApplicationEvents)
            {
                var subscriberApplicationEvent = _mapper.Map<SubscriberApplicationEventViewModel>(await _subscriberQueries.GetSubscribersApplicationEvent(item.SubscriberPartyId, item.ApplicationEventId));
                if (subscriberApplicationEvent == null)
                    await _mediatorHandler.Send(new CreateSubscriberApplicationEventCommand(item.SubscriberPartyId, item.ApplicationEventId, item.ChannelId));
                else
                    await _mediatorHandler.Send(new UpdateSubscriberApplicationEventCommand(item.SubscriberPartyId, item.ApplicationEventId, item.ChannelId));
            }

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Application Event Settings Updated", ToastType.Success);
                return RedirectToAction(nameof(ApplicationEventController.Index), "SubscriberApplicationEvent");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return RedirectToAction(nameof(ApplicationEventController.Index), "SubscriberApplicationEvent");
        }
    }
}