using AutoMapper;
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
using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Resources;
using WebMVC.Resources.Extensions;
using WebMVC.Services;
using WebMVC.ViewModels;

namespace WebMVC.Controllers
{
    [Authorize(AuthenticationSchemes = "OpenIdConnect")]
    public class MailBoxController : ControllerBase
    {
        private readonly ISubscriberQueries _subscriberQueries;
        private readonly IMailBoxRepository _mailBoxRepository;
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IMediatorHandler _mediatorHandler;
        public readonly IMapper _mapper;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;

        public MailBoxController(
            ISubscriberQueries subscriberQueries,
            INotificationHandler<DomainNotification> notifications,
            IMailBoxRepository mailBoxRepository,
            ISubscriberRepository subscriberRepository,
            IMapper mapper,
            IIdentityParser<ApplicationUser> appUserParser,
            IMediatorHandler mediatorHandler)

            : base(notifications, mediatorHandler)
        {
            _subscriberRepository = subscriberRepository ?? throw new ArgumentNullException(nameof(subscriberRepository));
            _subscriberQueries = subscriberQueries ?? throw new ArgumentNullException(nameof(subscriberQueries));
            _mailBoxRepository = mailBoxRepository ?? throw new ArgumentNullException(nameof(mailBoxRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _appUserParser = appUserParser ?? throw new ArgumentNullException(nameof(appUserParser));
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler));
        }

        public async Task<IActionResult> Inbox()
        {
            var mailBox = new MailBoxViewModel();
            var user = _appUserParser.Parse(HttpContext.User);

            mailBox.subscriber = _mapper.Map<SubscriberViewModel>(await _subscriberRepository.GetByIdAsync(user.PartyId));

            mailBox.MailBoxItems = _mapper.Map<IEnumerable<MailBoxItemViewModel>>(await _mailBoxRepository.GetEmailBox(user.PartyId));
            foreach (var item in mailBox.MailBoxItems)
            {
                item.Senders = _mapper.Map<IEnumerable<SubscriberViewModel>>(await _subscriberRepository.GetManyAsync(x => item.SenderPartyIds.Contains(x.SubscriberPartyId)));
            }

            return View("_MailBoxPartial", mailBox.MailBoxItems);
        }

        public async Task<IActionResult> SentEmails()
        {
            var mailBox = new MailBoxViewModel();
            var user = _appUserParser.Parse(HttpContext.User);

            mailBox.subscriber = _mapper.Map<SubscriberViewModel>(await _subscriberRepository.GetByIdAsync(user.PartyId));

            mailBox.MailBoxItems = _mapper.Map<IEnumerable<MailBoxItemViewModel>>(await _mailBoxRepository.GetDirectSentEmails(user.PartyId));
            foreach (var item in mailBox.MailBoxItems)
            {
                item.Senders = _mapper.Map<IEnumerable<SubscriberViewModel>>(await _subscriberRepository.GetManyAsync(x => item.SenderPartyIds.Contains(x.SubscriberPartyId)));
            }

            return View("_SentEmailPartial", mailBox.MailBoxItems);
        }

        public async Task<IActionResult> Trash()
        {
            var mailBox = new MailBoxViewModel();
            var user = _appUserParser.Parse(HttpContext.User);

            mailBox.subscriber = _mapper.Map<SubscriberViewModel>(await _subscriberRepository.GetByIdAsync(user.PartyId));

            mailBox.MailBoxItems = _mapper.Map<IEnumerable<MailBoxItemViewModel>>(await _mailBoxRepository.GetDeletedEmails(user.PartyId));
            foreach (var item in mailBox.MailBoxItems)
            {
                item.Senders = _mapper.Map<IEnumerable<SubscriberViewModel>>(await _subscriberRepository.GetManyAsync(x => item.SenderPartyIds.Contains(x.SubscriberPartyId)));
            }

            return View("_TrashPartialView", mailBox.MailBoxItems);
        }

        public async Task<IActionResult> EmailView(Guid messageId)
        {
            var user = _appUserParser.Parse(HttpContext.User);
            var mailBox = new MailBoxViewModel();

            mailBox.MailItem = _mapper.Map<MailBoxItemViewModel>(await _mailBoxRepository.GetWithRelatedByIdAsync(messageId, user.PartyId));
            mailBox.MailItem.Senders = _mapper.Map<IEnumerable<SubscriberViewModel>>(await _subscriberRepository.GetManyAsync(x => mailBox.MailItem.SenderPartyIds.Contains(x.SubscriberPartyId)));
                        
            mailBox.MailItem.Read = true;
            await _mediatorHandler.Send(new MailReadCommand(mailBox.MailItem.MessageId, user.PartyId));
            return View("_EmailViewPartial", mailBox.MailItem);
        }

        public async Task<IActionResult> MessageAcknowledged(Guid messageId)
        {
            var user = _appUserParser.Parse(HttpContext.User);
            var command = new MessageAcknowledgedCommand(messageId, user.PartyId);

            var result = await _mediatorHandler.Send(command);

            if (IsValidOperation())
                this.AddToastMessage("Success", "Message Acknowledged", ToastType.Success);
            else
                this.AddToastMessage("Error", "Error processing request", ToastType.Error);

            return RedirectToAction(nameof(MailBoxController.EmailView), new { messageId = command.MessageId });
        }

        public async Task<JsonResult> MoveToTrash(List<string> selectedMessageIds)
        {
            var user = _appUserParser.Parse(HttpContext.User);

            foreach (var message in selectedMessageIds)
            {
                var command = new MessageMarkAsDeletedCommand(Guid.Parse(message), user.PartyId);
                await _mediatorHandler.Send(command);
            }

            return IsValidOperation()
                ? Json(new { result = true, responseText = "Messages moved to trash" })
                : Json(new { result = false, responseText = GetFirstErrorMsg() });
        }

        public async Task<JsonResult> MarkAsRead(List<string> selectedMessageIds)
        {
            var user = _appUserParser.Parse(HttpContext.User);

            foreach (var message in selectedMessageIds)
            {
                var command = new MessageMarkAsReadCommand(Guid.Parse(message), user.PartyId);
                await _mediatorHandler.Send(command);
            }

            return IsValidOperation()
                ? Json(new { result = true, responseText = "Messages marked as Read" })
                : Json(new { result = false, responseText = GetFirstErrorMsg() });
        }

        public async Task<JsonResult> MoveToInbox(List<string> selectedMessageIds)
        {
            var user = _appUserParser.Parse(HttpContext.User);

            foreach (var message in selectedMessageIds)
            {
                var command = new MessageMoveToInboxCommand(Guid.Parse(message), user.PartyId);
                await _mediatorHandler.Send(command);
            }

            return IsValidOperation()
                ? Json(new { result = true, responseText = "Messages moved to Inbox" })
                : Json(new { result = false, responseText = GetFirstErrorMsg() });
        }

        [HttpGet]
        public async Task<IActionResult> ComposeEmail()
        {
            var composeEmailViewModel = new ComposeEmailViewModel();
            composeEmailViewModel.SubscribersList = new SelectList(await _subscriberQueries.GetAllSubscribersAsync(), "SubscriberPartyId", "Name");

            return View("_ComposeEmailPartial", composeEmailViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ComposeEmailViewModel model)
        {
            var user = _appUserParser.Parse(HttpContext.User);
            model.SenderPartyId = user.PartyId;
            model.RequireAcknowledgement = false;

            foreach (var recipientPartyId in model.RecipientPartyIds)
            {
                var command = new SendDirectEmailCommand(Guid.NewGuid(), model.SenderPartyId, recipientPartyId, model.Subject, model.Body, model.RequireAcknowledgement, DateTime.Now);
                await _mediatorHandler.Send(command);
            }

            if (IsValidOperation())
            {
                this.AddToastMessage("Success", "Message Sent", ToastType.Success);
                return RedirectToAction("Inbox", "MailBox");
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