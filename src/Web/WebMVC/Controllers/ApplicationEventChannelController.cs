using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notifications.Core.Mediator;
using Notifications.Core.Notifications;
using Subscribing.Application.Commands;
using Subscribing.Application.Queries;
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Resources;
using WebMVC.Resources.Extensions;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "OpenIdConnect")]
    public class ApplicationEventChannelController : ControllerBase
    {
        private readonly IApplicationEventQueries _applicationEventQueries;
        private readonly IMediatorHandler _mediatorHandler;
        private readonly IMapper _mapper;
        private readonly IChannelRepository _channelRepository;
        public IApplicationEventChannelRepository _applicationEventChannelRepository;

        public ApplicationEventChannelController(
            INotificationHandler<DomainNotification> notifications,

            IApplicationEventQueries applicationEventQueries,
            IMediatorHandler mediatorHandler,
            IMapper mapper,
            IChannelRepository channelRepository,
            IApplicationEventChannelRepository applicationEventChannelRepository)
            : base(notifications, mediatorHandler)
        {
            _applicationEventQueries = applicationEventQueries ?? throw new ArgumentNullException(nameof(applicationEventQueries));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _channelRepository = channelRepository ?? throw new ArgumentNullException(nameof(channelRepository));
            _applicationEventChannelRepository = applicationEventChannelRepository ?? throw new ArgumentNullException(nameof(applicationEventChannelRepository));
        }

        public async Task<IActionResult> Index(Guid applicationEventId)
        {
            var allChannels = _mapper.Map<IEnumerable<ChannelViewModel>>(
                await _channelRepository.GetAllAsync()).Where(x => x.ChannelName != "NotificationsApp");

            var applicationEvent = await _applicationEventQueries.GetApplicationEventByIdAsync(applicationEventId);

            var applicationEventChannelCreateViewModel = new ApplicationEventChannelCreateViewModel();
            applicationEventChannelCreateViewModel.ApplicationEvent = applicationEvent;

            foreach (var channel in allChannels)
            {
                var applicationEventChannel = _mapper.Map<ApplicationEventChannelViewModel>(await _applicationEventChannelRepository.GetByApplicationEventIdAndChannelAsync(applicationEventId, channel.Id));

                if (applicationEventChannel == null)
                    applicationEventChannel = new ApplicationEventChannelViewModel()
                    { ApplicationEventId = applicationEvent.Id, Channel = channel, ChannelId = channel.Id, DelayedSendMinutes = 0, Enabled = false }; // Create Default

                applicationEventChannelCreateViewModel.ApplicationEventChannelViewModels.Add(applicationEventChannel);
            }

            return View(applicationEventChannelCreateViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Save(ApplicationEventChannelCreateViewModel model)
        {
            foreach (var item in model.ApplicationEventChannelViewModels)
            {
                var applicationEventChannel = _mapper.Map<ApplicationEventChannelViewModel>(await _applicationEventChannelRepository.GetByApplicationEventIdAndChannelAsync(item.ApplicationEventId, item.ChannelId));
                if (applicationEventChannel == null)
                    await _mediatorHandler.Send(new CreateApplicationEventChannelCommand(item.ApplicationEventId, item.ChannelId, item.DelayedSendMinutes, item.Enabled, item.RequireAcknowledgement));
                else
                    await _mediatorHandler.Send(new UpdateApplicationEventChannelCommand(item.ApplicationEventId, item.ChannelId, item.DelayedSendMinutes, item.Enabled, item.RequireAcknowledgement));
            }

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Application Event Settings Updated", ToastType.Success);
                return RedirectToAction(nameof(ApplicationEventController.Index), "ApplicationEvent");
            }

            foreach (var error in GetNotifications())
            {
                ModelState.AddModelError("Error", error);
                this.AddToastMessage("Error", error, ToastType.Error);
            }

            return RedirectToAction(nameof(ApplicationEventController.Index), "ApplicationEvent");
        }
    }
}