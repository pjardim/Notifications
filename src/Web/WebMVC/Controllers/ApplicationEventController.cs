using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "OpenIdConnect")]
    public class ApplicationEventController : ControllerBase
    {
        private readonly IApplicationEventQueries _applicationEventQueries;
        private readonly IMediatorHandler _mediatorHandler;
        public IApplicationEventRepository _applicationEventRepository;

        public ApplicationEventController(
            INotificationHandler<DomainNotification> notifications,

            IApplicationEventQueries applicationEventQueries,
            IMediatorHandler mediatorHandler,
            IApplicationEventRepository applicationEventRepository)
            : base(notifications, mediatorHandler)
        {
            _applicationEventQueries = applicationEventQueries ?? throw new ArgumentNullException(nameof(applicationEventQueries));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _applicationEventRepository = applicationEventRepository ?? throw new ArgumentNullException(nameof(applicationEventRepository));
        }

        public async Task<IActionResult> Index()
        {
            return View(await _applicationEventQueries.GetAllApplicationEventsWithRelatedAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var applicationEvent = await _applicationEventQueries.GetApplicationEventWithRelatedAsync(id);
            if (applicationEvent == null) return BadRequest();

            applicationEvent.SubscriberFiltersList = new SelectList(
                await _applicationEventRepository.GetAllSubscriberFiltersAsync(), "Id", "FilterType");

            return View(applicationEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationEventViewModel model)
        {
            var command = new UpdateApplicationEventCommand(model.Id, model.ApplicationEventName, model.Description);

            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Application Event Updated", ToastType.Success);
                return RedirectToAction("Index", "ApplicationEvent");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
            }
            this.AddToastMessage("Error", GetFirstErrorMsg(), ToastType.Error);

            return RedirectToAction("Create", "ApplicationEvent");
        }

        public IActionResult Create()
        {
            var applicationEvent = new ApplicationEventCreateViewModel();

            applicationEvent.SubscriberFiltersList = new SelectList(
                 _applicationEventQueries.GetAllSubscriberFilterTypes(), "Id", "FilterType");

            return View(applicationEvent);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationEventCreateViewModel model)
        {
            var command = new CreateApplicationEventCommand(Guid.NewGuid(), model.ApplicationEventName, model.Description);

            await _mediatorHandler.Send(command);

            //Add SubscriberFilter
            foreach (var subscriberFilter in model.SubscriberFilters)
            {
                var addSubscriberFilterCommand = new AddSubscriberFilterCommand(Guid.NewGuid(), subscriberFilter.ApplicationEventId, subscriberFilter.FilterType, subscriberFilter.FilterValue);
                await _mediatorHandler.Send(addSubscriberFilterCommand);
            }

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Application Event Created", ToastType.Success);
                return RedirectToAction("Index", "ApplicationEvent");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return RedirectToAction("Create", "ApplicationEvent");
        }

        public async Task<IActionResult> Details(Guid id)
        {
            return View(await _applicationEventQueries.GetApplicationEventByIdAsync(id));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            return View(await _applicationEventQueries.GetApplicationEventByIdAsync(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var command = new RemoveApplicationEventCommand(id);

            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Application Event Deleted", ToastType.Success);
                return RedirectToAction("Index", "ApplicationEvent");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return RedirectToAction("Delete", "ApplicationEvent");
        }
    }
}