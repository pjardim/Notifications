using FluentValidation.Results;
using MediatR;
using System;


namespace Notifications.Core.Messaging
{
    public abstract class Command : DomainMessage, IRequest<bool>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}