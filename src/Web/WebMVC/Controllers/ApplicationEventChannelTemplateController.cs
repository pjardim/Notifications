using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Notifications.Core.Mediator;
using Notifications.Core.Notifications;
using Subscribing.Application.Commands;
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "OpenIdConnect")]
    public class ApplicationEventChannelTemplateController : ControllerBase
    {
        private readonly IMediatorHandler _mediatorHandler;

        public readonly IChannelRepository _channelRepository;
        public readonly IMapper _mapper;
        public readonly IConfiguration _configuration;
        public readonly IApplicationEventChannelTemplateRepository _applicationEventChannelTemplateRepository;
        public readonly IApplicationEventRepository _applicationEventRepository;

        public ApplicationEventChannelTemplateController(
            INotificationHandler<DomainNotification> notifications,
            IMapper mapper,
            IConfiguration configuration,
            IApplicationEventChannelTemplateRepository applicationEventChannelTemplateRepository,
            IApplicationEventRepository applicationEventRepository,
            IMediatorHandler mediatorHandler,
            IChannelRepository channelRepository
           )
            : base(notifications, mediatorHandler)
        {
            _mapper = mapper;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _applicationEventChannelTemplateRepository = applicationEventChannelTemplateRepository ?? throw new ArgumentNullException(nameof(applicationEventChannelTemplateRepository));
            _applicationEventRepository = applicationEventRepository ?? throw new ArgumentNullException(nameof(applicationEventRepository));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
            _channelRepository = channelRepository ?? throw new ArgumentNullException(nameof(channelRepository));
        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid applicationEventId, Guid channelId)
        {
            var channels = await _channelRepository.GetAllAsync();
            var selectedChannel = channels.FirstOrDefault(x => x.Id == channelId) ?? channels.FirstOrDefault();

            var applicationEventChannelTemplate = _mapper.Map<ApplicationEventChannelTemplateViewModel>(await _applicationEventChannelTemplateRepository
                .GetByApplicationEventAndChannel(applicationEventId, selectedChannel.Id)) ?? new ApplicationEventChannelTemplateViewModel();

            //In Case of a new  template get the template parameters
            if (applicationEventChannelTemplate.ApplicationEvent == null)
            {
                var applicationEventWithApplicationEventParameters = await _applicationEventRepository
                    .GetManyAsync(x => x.Id == applicationEventId, includeProperties: "ApplicationEventParameters");

                applicationEventChannelTemplate.ApplicationEvent = _mapper.Map<ApplicationEventViewModel>(applicationEventWithApplicationEventParameters.FirstOrDefault());
                applicationEventChannelTemplate.ApplicationEventId = applicationEventChannelTemplate.ApplicationEvent.Id;
            }

            applicationEventChannelTemplate.ChannelsList = new SelectList(channels, "Id", "ChannelName");

            return View(applicationEventChannelTemplate);
        }

        [HttpPost]
        public async Task<JsonResult> SaveTemplate(Guid applicationEventId, Guid channelId, string subject, string body)
        {
            var applicationEventChannelTemplate = _mapper.Map<ApplicationEventChannelTemplateViewModel>(await _applicationEventChannelTemplateRepository.GetByApplicationEventAndChannel(applicationEventId, channelId));
            if (applicationEventChannelTemplate == null)
                await _mediatorHandler.Send(new CreateApplicationEventChannelTemplateCommand(applicationEventId, channelId, "", "", subject, body));
            else
                await _mediatorHandler.Send(new UpdateApplicationEventChannelTemplateCommand(applicationEventId, channelId, "", "", subject, body));

            return IsValidOperation()
               ? Json(new { result = true, responseText = "Template  Created" })
               : Json(new { result = false, responseText = GetFirstErrorMsg() });
        }
    }
}