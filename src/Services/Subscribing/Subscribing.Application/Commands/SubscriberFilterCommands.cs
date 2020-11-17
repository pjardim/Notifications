using FluentValidation;
using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Commands
{
    public class AddSubscriberFilterCommand : Command
    {
        public AddSubscriberFilterCommand(Guid id, Guid applicationEventId, string filterType, string filterValue)
        {
            Id = id;
            ApplicationEventId = applicationEventId;
            FilterType = filterType;
            FilterValue = filterValue;
        }

        public Guid Id { get; }
        public Guid ApplicationEventId { get; private set; }
        public string FilterType { get; private set; }
        public string FilterValue { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new AddSubscriberFilterCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddSubscriberFilterCommandValidation : AbstractValidator<AddSubscriberFilterCommand>
    {
        public AddSubscriberFilterCommandValidation()
        {
            RuleFor(c => c.FilterType)
           .NotEqual(string.Empty)
           .WithMessage("FilterType can't be Empty");

            RuleFor(c => c.FilterValue)
             .NotEqual(string.Empty)
            .WithMessage("FilterType can't be Empty");

            RuleFor(c => c.ApplicationEventId)
                .NotEqual(Guid.Empty)
                .WithMessage("Application Event Invalid");
        }
    }
}