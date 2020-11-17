using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Notifications.Core.Mediator;
using Notifications.Core.Notifications;
using Subscribing.Application.Commands;
using Subscribing.Application.Queries;
using Subscribing.Application.Queries.ViewModels;
using System;
using System.Threading.Tasks;
using WebMVC.Resources;
using WebMVC.Resources.Extensions;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "OpenIdConnect")]
    public class ApplicationEventParameterController : ControllerBase
    {
        private readonly IApplicationEventQueries _applicationEventQueries;
        private readonly IMediatorHandler _mediatorHandler;

        public ApplicationEventParameterController(
            INotificationHandler<DomainNotification> notifications,
            IApplicationEventQueries applicationEventQueries,
            IMediatorHandler mediatorHandler)

            : base(notifications, mediatorHandler)
        {
            _applicationEventQueries = applicationEventQueries ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<IActionResult> Index(Guid applicationEventId)
        {
            var applicationEventPamrameters = await _applicationEventQueries.GetAllApplicationParametersByApplicationEventIdAsync(applicationEventId);

            var viewModel = new ApplicationEventParameterIndexViewModel();
            viewModel.ApplicationEventId = applicationEventId;
            viewModel.ApplicationEventParameters = applicationEventPamrameters;


            return View(viewModel);
        }

        public async Task<IActionResult> UpdateParameters(Guid applicationEventId)
        {
            //Todo Update all parameters based on reflection
            var applicationEvent = await _applicationEventQueries.GetApplicationEventByIdAsync(applicationEventId);
            if (applicationEvent != null)
            {
                var applicationEventParameters = await _applicationEventQueries.GetAllApplicationParametersByApplicationEventIdAsync(applicationEventId);

                Type applicationEventType = Type.GetType("Notifications.Core.ApplicationEvents." + applicationEvent.ApplicationEventName + ", Notifications.Core");
                if (applicationEventType != null)
                {
                    //
                }

                return View(await _applicationEventQueries.GetAllApplicationEventsWithRelatedAsync());
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var applicationEventParameter = await _applicationEventQueries.GetApplicationEventParameterById(id);
            if (applicationEventParameter == null) return BadRequest();

            return View(applicationEventParameter);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ApplicationEventParameterViewModel model)
        {
            var command = new UpdateApplicationEventParameterCommand(model.Id, model.ApplicationEventId, model.ParameterName, model.Description);

            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Application Event Parameter Updated", ToastType.Success);
                return RedirectToAction("Index", "ApplicationEventParameter", new { applicationEventId = command.ApplicationEventId });
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
            }
            this.AddToastMessage("Error", GetFirstErrorMsg(), ToastType.Error);

            return RedirectToAction("Create", "ApplicationEventParameter", new { applicationEventId = command.ApplicationEventId });
        }

        public async Task<IActionResult> Create(Guid applicationEventId)
        {
            var applicationEvent = await _applicationEventQueries.GetApplicationEventByIdAsync(applicationEventId);
            var applicationEventParameter = new ApplicationEventParameterViewModel();

            applicationEventParameter.ApplicationEvent = applicationEvent;

            applicationEventParameter.ApplicationEventList = new SelectList(
              await _applicationEventQueries.GetAllApplicationEventsAsync(), "Id", "ApplicationEventName");

            if (applicationEventParameter == null) return BadRequest();

            return View(applicationEventParameter);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationEventParameterViewModel model)
        {
            var command = new CreateApplicationEventParameterCommand(model.ApplicationEventId, model.ParameterName, model.Description);

            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Application Event Parameter Created", ToastType.Success);
                return RedirectToAction("Index", "ApplicationEventParameter", new { applicationEventId = command.ApplicationEventId });
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return RedirectToAction("Create", "ApplicationEventParameter", new { applicationEventId = command.ApplicationEventId });
        }

        public async Task<IActionResult> Details(Guid id)
        {
            return View(await _applicationEventQueries.GetApplicationEventParameterById(id));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            return View(await _applicationEventQueries.GetApplicationEventParameterById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var applicationEventParameter = await _applicationEventQueries.GetApplicationEventParameterById(id);

            var command = new RemoveApplicationEventParameterCommand(id);

            await _mediatorHandler.Send(command);

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Application Event Parameter Deleted", ToastType.Success);
                return RedirectToAction("Index", "ApplicationEventParameter", new { applicationEventId = applicationEventParameter.ApplicationEventId });
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return RedirectToAction("Delete", "ApplicationEventParameter", new { applicationEventId = applicationEventParameter.ApplicationEventId });
        }
    }
}