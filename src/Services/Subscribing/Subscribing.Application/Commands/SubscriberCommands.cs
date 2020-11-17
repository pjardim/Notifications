using FluentValidation;
using Notifications.Core.Messaging;

namespace Subscribing.Application.Commands
{
    public class CreateSubscriberCommand : Command
    {
        public CreateSubscriberCommand(string subscriberPartyId, string email, string name)
        {
            SubscriberPartyId = subscriberPartyId;
            Email = email;
            Name = name;
        }

        public string SubscriberPartyId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateSubscriberValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateSubscriberCommand : Command
    {
        public UpdateSubscriberCommand(string subscriberPartyId, string email, string name)
        {
            SubscriberPartyId = subscriberPartyId;
            Email = email;
            Name = name;
        }

        public string SubscriberPartyId { get; private set; }
        public string Email { get; private set; }
        public string Name { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateSubscriberValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RemoveSubscriberCommand : Command
    {
        public virtual string SubscriberPartyId { get; set; }

        public RemoveSubscriberCommand(string subscriberPartyId)
        {
            SubscriberPartyId = subscriberPartyId;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveSubscriberEventValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateSubscriberValidation : AbstractValidator<CreateSubscriberCommand>
    {
        public CreateSubscriberValidation()
        {
            RuleFor(c => c.SubscriberPartyId)
             .NotEqual(string.Empty)
             .WithMessage("Id Invalid");

            RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Email Invalid");

            RuleFor(c => c.Name)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Name must have between 2 and 100 characters");
        }
    }

    public class UpdateSubscriberValidation : AbstractValidator<UpdateSubscriberCommand>
    {
        public UpdateSubscriberValidation()
        {
            RuleFor(c => c.SubscriberPartyId)
             .NotEqual(string.Empty)
             .WithMessage("Id Invalid");

            RuleFor(c => c.Email)
            .EmailAddress()
            .WithMessage("Email Invalid");

            RuleFor(c => c.Name)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Name must have between 2 and 100 characters");
        }
    }

    public class RemoveSubscriberEventValidation : AbstractValidator<RemoveSubscriberCommand>
    {
        public RemoveSubscriberEventValidation()
        {
            RuleFor(c => c.SubscriberPartyId)
               .NotEqual(string.Empty)
               .WithMessage("Id Invalid");
        }
    }
}