using FluentValidation;
using Notifications.Core.Messaging;
using System.Collections.Generic;

namespace Subscribing.Application.Commands
{
    public class SaveSubscriberGroupSubscriberCommand : Command
    {
        public SaveSubscriberGroupSubscriberCommand(string subscriberPartyId, List<int> subscriberGroups)
        {
            SubscriberPartyId = subscriberPartyId;
            SubscriberGroups = subscriberGroups;
        }

        public string SubscriberPartyId { get; private set; }

        public List<int> SubscriberGroups { get; private set; }

        public override bool IsValid()
        {
            ValidationResult = new SaveSubscriberGroupSubscriberValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class SaveSubscriberGroupSubscriberValidation : AbstractValidator<SaveSubscriberGroupSubscriberCommand>
    {
        public SaveSubscriberGroupSubscriberValidation()
        {
            RuleFor(c => c.SubscriberPartyId)
                 .NotEqual(string.Empty)
                 .WithMessage("SubscriberPartyId Cannot be empty");
        }
    }
}