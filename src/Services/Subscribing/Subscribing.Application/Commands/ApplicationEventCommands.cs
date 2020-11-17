using FluentValidation;
using Notifications.Core.Messaging;
using Subscribing.Domain;
using System;
using System.Collections.Generic;

namespace Subscribing.Application.Commands
{
    public class CreateApplicationEventCommand : Command
    {
        public Guid Id { get; set; }
        public string ApplicationEventName { get; private set; }
        public string Description { get; private set; }
        public IEnumerable<SubscriberFilter> SubscriberFilters { get; private set; }

        public CreateApplicationEventCommand(Guid ApplicationEventId, string applicationEventName, string description)
        {
            ApplicationEventName = applicationEventName;
            Description = description;
            AggregateId = ApplicationEventId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateApplicationEventValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateApplicationEventCommand : Command
    {
        public Guid Id { get; set; }
        public string ApplicationEventName { get; private set; }
        public string Description { get; private set; }

        public UpdateApplicationEventCommand(Guid id, string applicationEventName, string description)
        {
            Id = id;
            ApplicationEventName = applicationEventName;
            Description = description;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateApplicationEventValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateApplicationEventValidation : AbstractValidator<CreateApplicationEventCommand>
    {
        public CreateApplicationEventValidation()
        {
            RuleFor(c => c.ApplicationEventName)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Name must have between 2 and 100 characters");
        }
    }

    public class RemoveApplicationEventCommand : Command
    {
        public virtual Guid Id { get; set; }

        public RemoveApplicationEventCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveApplicationEventValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RemoveApplicationEventValidation : AbstractValidator<RemoveApplicationEventCommand>
    {
        public RemoveApplicationEventValidation()
        {
            RuleFor(c => c.Id)
               .NotEqual(Guid.Empty)
               .WithMessage("Id Invalid");
        }
    }

    public class UpdateApplicationEventValidation : AbstractValidator<UpdateApplicationEventCommand>
    {
        public UpdateApplicationEventValidation()
        {
            RuleFor(c => c.Id)
               .NotEqual(Guid.Empty).WithMessage("Id Invalid");

            RuleFor(c => c.ApplicationEventName)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Name must have between 2 and 100 characters");
        }
    }
}