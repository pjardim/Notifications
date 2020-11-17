using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Notifications.Core.Mediator;
using Notifications.Core.Notifications;
using Subscribing.Application.Commands;
using Subscribing.Application.Queries;
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
    public class SubscriberGroupController : ControllerBase
    {
        private readonly ISubscriberQueries _subscriberQueries;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;
        public ISubscriberRepository _subscriberRepository;

        public SubscriberGroupController(
            INotificationHandler<DomainNotification> notifications,
            ISubscriberQueries subscriberQueries,
            IMediatorHandler mediatorHandler,
            IIdentityParser<ApplicationUser> appUserParser,
            ISubscriberRepository subscriberRepository)
            : base(notifications, mediatorHandler)
        {
            _subscriberQueries = subscriberQueries ?? throw new ArgumentNullException(nameof(subscriberQueries));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _appUserParser = appUserParser;
            _subscriberRepository = subscriberRepository ?? throw new ArgumentNullException(nameof(subscriberRepository));
        }

        public async Task<IActionResult> Index(string subscriberPartyId)
        {
            var selectedPartyId = subscriberPartyId ?? _appUserParser.Parse(HttpContext.User).PartyId;
            var subscriberGroupSettings = new SubscriberGroupSettingsViewModel();
            subscriberGroupSettings.SubscriberPartyId = selectedPartyId;

            var allUserGroups = await _subscriberQueries.GetAllSubscriberGroupSubscribersByPartyIdAsync(selectedPartyId);

            subscriberGroupSettings.SubscribersList = new SelectList(await _subscriberQueries.GetAllSubscribersAsync(), "SubscriberPartyId", "Name");
            subscriberGroupSettings.SubscriberGroups = new SelectList(_subscriberQueries.GetAllSubscriberGroups(), "Id", "Name");
            subscriberGroupSettings.Subscriber = await _subscriberQueries.GetSubscriberByPartyIdAsync(selectedPartyId);
            subscriberGroupSettings.SelectedGroups = allUserGroups.Select(x => x.SubscriberGroupId).ToList();

            return View(subscriberGroupSettings);
        }

        [HttpPost]
        public async Task<IActionResult> Save(SubscriberGroupSettingsViewModel model)
        {
            var command = new SaveSubscriberGroupSubscriberCommand(model.SubscriberPartyId, model.SelectedGroups);
            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Subscriber Groups Updated", ToastType.Success);
                return RedirectToAction("Index", "SubscriberGroup");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return View(model);
        }
    }
}