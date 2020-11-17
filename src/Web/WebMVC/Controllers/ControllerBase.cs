using MediatR;
using Microsoft.AspNetCore.Mvc;
using Notifications.Core.Mediator;
using Notifications.Core.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebMVC.Controllers
{
    public abstract class ControllerBase : Controller
    {
        private readonly DomainNotificationHandler _notifications;
        private readonly IMediatorHandler _mediatorHandler;

        protected ControllerBase(INotificationHandler<DomainNotification> notifications,
                                 IMediatorHandler mediatorHandler)
        {
            _notifications = (DomainNotificationHandler)notifications;
            _mediatorHandler = mediatorHandler ?? throw new ArgumentNullException(nameof(mediatorHandler)); ;
        }

        protected bool IsValidOperation()
        {
            return !_notifications.HasNotifications();
        }

        protected IEnumerable<string> GetNotifications()
        {
            return _notifications.GetNotifications().Select(c => c.Value).ToList();
        }

        protected string GetFirstErrorMsg()
        {
            return _notifications.GetNotifications().Select(c => c.Value).FirstOrDefault();
        }

        protected void NotifyError(string code, string message)
        {
            _mediatorHandler.PublishNotification(new DomainNotification(code, message));
        }

    }
}