using FluentValidation;
using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Commands
{
    public class CreateApplicationEventChannelCommand : Command
    {
        public CreateApplicationEventChannelCommand(Guid applicationEventId, Guid channelId, int delayedSendMinutes, bool enabled, bool requireAcknowledgement)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            DelayedSendMinutes = delayedSendMinutes;
            Enabled = enabled;
            RequireAcknowledgement = requireAcknowledgement;
            AggregateId = applicationEventId;
        }

        public Guid ApplicationEventId { get; private set; }
        public Guid ChannelId { get; private set; }

        public int DelayedSendMinutes { get; private set; }
        public bool Enabled { get; private set; }
        public bool RequireAcknowledgement { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new CreateApplicationEventChannelValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateApplicationEventChannelCommand : Command
    {
        public UpdateApplicationEventChannelCommand(Guid applicationEventId, Guid channelId, int delayedSendMinutes, bool enabled, bool requireAcknowledgement)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            DelayedSendMinutes = delayedSendMinutes;
            Enabled = enabled;
            RequireAcknowledgement = requireAcknowledgement;
            AggregateId = applicationEventId;
        }

        public Guid ApplicationEventId { get; private set; }
        public Guid ChannelId { get; private set; }

        public int DelayedSendMinutes { get; private set; }
        public bool Enabled { get; private set; }
        public bool RequireAcknowledgement { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new UpdateApplicationEventChannelValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateApplicationEventChannelValidation : AbstractValidator<CreateApplicationEventChannelCommand>
    {
        public CreateApplicationEventChannelValidation()
        {
            RuleFor(c => c.ApplicationEventId)
                 .NotEqual(Guid.Empty)
                 .WithMessage("ApplicationEventId Cannot be null");

            RuleFor(c => c.ChannelId)
                  .NotEqual(Guid.Empty)
                  .WithMessage("ChannelId Cannot be null");

            RuleFor(c => c.DelayedSendMinutes)
               .GreaterThanOrEqualTo(0)
                .WithMessage("The Delay Time should be greater than or equal to to 0");

            RuleFor(c => c.DelayedSendMinutes)
              .LessThanOrEqualTo(1440)
               .WithMessage("The Delay Time should be less than or equal to to 1440 (24 hours)");
        }
    }

    public class UpdateApplicationEventChannelValidation : AbstractValidator<UpdateApplicationEventChannelCommand>
    {
        public UpdateApplicationEventChannelValidation()
        {
            RuleFor(c => c.ApplicationEventId)
                 .NotEqual(Guid.Empty)
                 .WithMessage("ApplicationEventId Cannot be null");

            RuleFor(c => c.ChannelId)
                  .NotEqual(Guid.Empty)
                  .WithMessage("ChannelId Cannot be null");

            RuleFor(c => c.DelayedSendMinutes)
               .GreaterThanOrEqualTo(0)
                .WithMessage("The Delay Time should be greater than or equal to to 0");

            RuleFor(c => c.DelayedSendMinutes)
              .LessThanOrEqualTo(1440)
               .WithMessage("The Delay Time should be less than or equal to to 1440 (24 hours)");
        }
    }
}