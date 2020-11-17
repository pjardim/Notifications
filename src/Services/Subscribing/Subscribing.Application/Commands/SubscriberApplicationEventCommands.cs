using FluentValidation;
using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Commands
{
    public class CreateSubscriberApplicationEventCommand : Command
    {
        public CreateSubscriberApplicationEventCommand(string subscriberPartyId, Guid applicationEventId, Guid channelId)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            SubscriberPartyId = subscriberPartyId;
            AggregateId = applicationEventId;
        }

        public string SubscriberPartyId { get; set; }

        public Guid ApplicationEventId { get; set; }
        public Guid ChannelId { get; set; }


        public override bool IsValid()
        {
            ValidationResult = new CreateSubscriberApplicationEventValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateSubscriberApplicationEventCommand : Command
    {
        public UpdateSubscriberApplicationEventCommand(string subscriberPartyId, Guid applicationEventId, Guid channelId)
        {
            SubscriberPartyId = subscriberPartyId;
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            AggregateId = applicationEventId;
        }

        public string SubscriberPartyId { get; set; }
        public Guid ApplicationEventId { get; set; }
        public Guid ChannelId { get; set; }


        public override bool IsValid()
        {
            ValidationResult = new UpdateSubscriberApplicationEventValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateSubscriberApplicationEventValidation : AbstractValidator<CreateSubscriberApplicationEventCommand>
    {
        public CreateSubscriberApplicationEventValidation()
        {
            RuleFor(c => c.ApplicationEventId)
                 .NotEqual(Guid.Empty)
                 .WithMessage("ApplicationEventId Cannot be null");

            RuleFor(c => c.ChannelId)
                  .NotEqual(Guid.Empty)
                  .WithMessage("ChannelId Cannot be null");


            RuleFor(c => c.SubscriberPartyId)
                 .NotEqual(string.Empty)
                 .WithMessage("partyId Cannot be null");
        }
    }

    public class UpdateSubscriberApplicationEventValidation : AbstractValidator<UpdateSubscriberApplicationEventCommand>
    {
        public UpdateSubscriberApplicationEventValidation()
        {
            RuleFor(c => c.ApplicationEventId)
                 .NotEqual(Guid.Empty)
                 .WithMessage("ApplicationEventId Cannot be null");

            RuleFor(c => c.ChannelId)
                  .NotEqual(Guid.Empty)
                  .WithMessage("ChannelId Cannot be null");

            RuleFor(c => c.SubscriberPartyId)
                 .NotEqual(string.Empty)
                 .WithMessage("partyId Cannot be null");


        }
    }
}