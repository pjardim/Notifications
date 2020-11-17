using FluentValidation;
using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Commands
{
    public class CreateNotificationTemplateCommand : Command
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; }

        public CreateNotificationTemplateCommand(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateNotificationTemplateValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateNotificationTemplateCommand : Command
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Description { get; }

        public UpdateNotificationTemplateCommand(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateNotificationTemplateValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateNotificationTemplateValidation : AbstractValidator<CreateNotificationTemplateCommand>
    {
        public CreateNotificationTemplateValidation()
        {
            RuleFor(c => c.Id)
             .NotEqual(Guid.Empty)
             .WithMessage("Id Invalid");

            RuleFor(c => c.Name)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Name must have between 2 and 100 characters");
        }
    }

    public class RemoveNotificationTemplateCommand : Command
    {
        public virtual Guid Id { get; set; }

        public RemoveNotificationTemplateCommand(Guid id)
        {
            Id = id;
            AggregateId = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveNotificationTemplateValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RemoveNotificationTemplateValidation : AbstractValidator<RemoveNotificationTemplateCommand>
    {
        public RemoveNotificationTemplateValidation()
        {
            RuleFor(c => c.Id)
               .NotEqual(Guid.Empty)
               .WithMessage("Id Invalid");
        }
    }

    public class UpdateNotificationTemplateValidation : AbstractValidator<UpdateNotificationTemplateCommand>
    {
        public UpdateNotificationTemplateValidation()
        {
            RuleFor(c => c.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Id Invalid");

            RuleFor(c => c.Name)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Name must have between 2 and 100 characters");
        }
    }
}