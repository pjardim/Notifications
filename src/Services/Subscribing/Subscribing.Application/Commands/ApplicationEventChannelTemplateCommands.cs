using FluentValidation;
using Notifications.Core.Messaging;
using System;

namespace Subscribing.Application.Commands
{
    public class CreateApplicationEventChannelTemplateCommand : Command
    {
        public Guid ApplicationEventId { get; private set; }
        public Guid ChannelId { get; private set; }
        public string Format { get; private set; }
        public string Encoding { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public CreateApplicationEventChannelTemplateCommand(Guid applicationEventId, Guid channelId, string format, string encoding, string subject, string body)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            Format = format;
            Encoding = encoding;
            Subject = subject;
            Body = body;
            AggregateId = applicationEventId;
        }

        public override bool IsValid()
        {
            ValidationResult = new CreateApplicationEventChannelTemplateValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class UpdateApplicationEventChannelTemplateCommand : Command
    {
        public Guid ApplicationEventId { get; private set; }
        public Guid ChannelId { get; private set; }
        public string Format { get; private set; }
        public string Encoding { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }

        public UpdateApplicationEventChannelTemplateCommand(Guid applicationEventId, Guid channelId, string format, string encoding, string subject, string body)
        {
            ApplicationEventId = applicationEventId;
            ChannelId = channelId;
            Format = format;
            Encoding = encoding;
            Subject = subject;
            Body = body;
            AggregateId = applicationEventId;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateApplicationEventChannelTemplateValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CreateApplicationEventChannelTemplateValidation : AbstractValidator<CreateApplicationEventChannelTemplateCommand>
    {
        public CreateApplicationEventChannelTemplateValidation()
        {
            RuleFor(c => c.ApplicationEventId)
                 .NotEqual(Guid.Empty)
                 .WithMessage("ApplicationEventId Cannot be null");

            RuleFor(c => c.ChannelId)
                  .NotEqual(Guid.Empty)                 
                  .WithMessage("ChannelId Cannot be null");

            RuleFor(c => c.Subject)
                 .NotEqual(string.Empty)
                  .NotNull()
                .WithMessage("Subject cannot be Empty");

            RuleFor(c => c.Body)
                .NotEqual(string.Empty)
                 .NotNull()
               .WithMessage("Body cannot be Empty");
        }
    }

    public class UpdateApplicationEventChannelTemplateValidation : AbstractValidator<UpdateApplicationEventChannelTemplateCommand>
    {
        public UpdateApplicationEventChannelTemplateValidation()
        {
            RuleFor(c => c.ApplicationEventId)
                 .NotEqual(Guid.Empty)
                 .WithMessage("ApplicationEventId Cannot be null");

            RuleFor(c => c.ChannelId)
                  .NotEqual(Guid.Empty)
                  .WithMessage("ChannelId Cannot be null");

            RuleFor(c => c.Subject)
               .NotEqual(string.Empty)
                .NotNull()
               .WithMessage("Subject cannot be Empty");

            RuleFor(c => c.Body)
                .NotEqual(string.Empty)
                 .NotNull()
                .WithMessage("Body cannot be Empty");
        }
    }
}