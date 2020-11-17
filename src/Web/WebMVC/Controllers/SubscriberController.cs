using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notifications.Core.Mediator;
using Notifications.Core.Notifications;
using Subscribing.Application.Commands;
using Subscribing.Application.Queries;
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Threading.Tasks;
using WebMVC.Resources;
using WebMVC.Resources.Extensions;

namespace WebMVC.Controllers
{
    public class SubscriberController : ControllerBase
    {
        private readonly ISubscriberQueries _subscriberQueries;
        private readonly IMediatorHandler _mediatorHandler;
        public ISubscriberRepository _subscriberRepository;

        public SubscriberController(
            INotificationHandler<DomainNotification> notifications,

            ISubscriberQueries subscriberQueries,
            IMediatorHandler mediatorHandler,
            ISubscriberRepository subscriberRepository)
            : base(notifications, mediatorHandler)
        {
            _subscriberQueries = subscriberQueries ?? throw new ArgumentNullException(nameof(subscriberQueries));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _subscriberRepository = subscriberRepository ?? throw new ArgumentNullException(nameof(subscriberRepository));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _subscriberQueries.GetAllSubscribersAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var subscriber = await _subscriberQueries.GetSubscriberByPartyIdAsync(id);
            if (subscriber == null) return BadRequest();

            return View(subscriber);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SubscriberViewModel model)
        {
            var command = new UpdateSubscriberCommand(model.SubscriberPartyId, model.Email, model.Name);

            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Subscriber Updated", ToastType.Success);
                return RedirectToAction("Index", "Subscriber");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
            }
            this.AddToastMessage("Error", GetFirstErrorMsg(), ToastType.Error);

            return View(model);
        }

        public IActionResult Create()
        {
            var subscriber = new SubscriberViewModel();

            return View(subscriber);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SubscriberViewModel model)
        {
            var command = new CreateSubscriberCommand(model.SubscriberPartyId, model.Email, model.Name);

            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Subscriber Created", ToastType.Success);
                return RedirectToAction("Index", "Subscriber");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            return View(await _subscriberQueries.GetSubscriberByPartyIdAsync(id));
        }

        public async Task<IActionResult> Delete(string id)
        {
            return View(await _subscriberQueries.GetSubscriberByPartyIdAsync(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var command = new RemoveSubscriberCommand(id);

            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Subscriber Deleted", ToastType.Success);
                return RedirectToAction("Index", "Subscriber");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return RedirectToAction("Delete", "Subscriber");
        }
    }
}