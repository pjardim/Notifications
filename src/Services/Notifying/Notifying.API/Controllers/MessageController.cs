using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Notifying.API.Application.Queries;
using Notifying.API.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Notifying.API.Controllers
{
    public class MessageController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly IIdentityService _identityService;
        private readonly IMessageQueries _messageQueries;
        private readonly ILogger<NotificationsController> _logger;

        public MessageController(
            IMediator mediator,
            IMessageQueries messageQueries,
            IIdentityService identityService,

            ILogger<NotificationsController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _messageQueries = messageQueries ?? throw new ArgumentNullException(nameof(messageQueries)); ;
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Message>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessagesWithNotificationsAsync()
        {
           
            var messages = await _messageQueries.GetPendingMessagesWithNotificationsAsync();

            return Ok(messages);
        }
    }
}