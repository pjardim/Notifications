using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notifying.API.Infrastructure.Services;
using System;

namespace Notifying.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IIdentityService _identityService;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(
            IMediator mediator,
            IIdentityService identityService,

            ILogger<NotificationsController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
    }
}