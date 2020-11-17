using FluentValidation;
using Notifications.Core.Messaging;
using Subscribing.Domain;
using System;

namespace Subscribing.Application.Commands
{
    public class CreateApplicationEventParameterCommand : Command
    {
        public CreateApplicationEventParameterCommand(Guid applicationEventId, string parameterName, string description)
        {
            ApplicationEventId = applicationEventId;
            ParameterName = parameterName;
            Description = description;
            AggregateId = applicationEventId;
        }

        public Guid Id { get; }
        public Guid ApplicationEventId { get; private set; }
        public ApplicationEvent ApplicationEvent { get; private set; }
        public string ParameterName { get; private set; }
        public string Description { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateApplicationEventParameterValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateApplicationEventParameterCommand : Command
    {
        public UpdateApplicationEventParameterCommand(Guid id, Guid applicationEventId, string parameterName, string description)
        {
            Id = id;
            ApplicationEventId = applicationEventId;
            ParameterName = parameterName;
            Description = description;
            AggregateId = applicationEventId;
        }

        public Guid Id { get; }
        public Guid ApplicationEventId { get; private set; }
        public string ParameterName { get; private set; }
        public string Description { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateApplicationEventParameterValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class RemoveApplicationEventParameterCommand : Command
    {
        public virtual Guid Id { get; set; }

        public RemoveApplicationEventParameterCommand(Guid id)
        {
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new RemoveApplicationEventParameterValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateApplicationEventParameterValidation : AbstractValidator<CreateApplicationEventParameterCommand>
    {
        public CreateApplicationEventParameterValidation()
        {
            RuleFor(c => c.Description)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Description must have between 2 and 100 characters");

            RuleFor(c => c.ParameterName)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Parameter Name must have between 2 and 100 characters");
        }
    }

    public class RemoveApplicationEventParameterValidation : AbstractValidator<RemoveApplicationEventParameterCommand>
    {
        public RemoveApplicationEventParameterValidation()
        {
            RuleFor(c => c.Id)
               .NotEqual(Guid.Empty)
               .WithMessage("Id Invalid");
        }
    }

    public class UpdateApplicationEventParameterValidation : AbstractValidator<UpdateApplicationEventParameterCommand>
    {
        public UpdateApplicationEventParameterValidation()
        {
            RuleFor(c => c.Description)
                 .NotEmpty()
                 .Length(2, 100).WithMessage("The Description must have between 2 and 100 characters");

            RuleFor(c => c.ParameterName)
                .NotEmpty()
                .Length(2, 100).WithMessage("The Parameter Name must have between 2 and 100 characters");
        }
    }
}