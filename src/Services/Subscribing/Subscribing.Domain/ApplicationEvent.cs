using Notifications.Core.SeedWork;
using Subscribing.Domain.Events;
using System;
using System.Collections.Generic;

namespace Subscribing.Domain
{
    public class ApplicationEvent : Entity, IAggregateRoot
    {
        protected ApplicationEvent()
        {
            SubscriberApplicationEvents = new List<SubscriberApplicationEvent>();
            ApplicationEventChannels = new List<ApplicationEventChannel>();
            _applicationEventParameters = new List<ApplicationEventParameter>();
            _subscriberFilters = new List<SubscriberFilter>();
            ApplicationEventChannelTemplates = new List<ApplicationEventChannelTemplate>();
        }

        public ApplicationEvent(Guid id, string applicationEventName, string description) : this()
        {
            Id = id;
            ApplicationEventName = applicationEventName;
            Description = description;

            Validate();
        }

        public string ApplicationEventName { get; private set; }
        public string Description { get; private set; }

        public Guid? NotificationTemplateId { get; private set; }

        public ICollection<ApplicationEventChannelTemplate> ApplicationEventChannelTemplates { get; private set; }

        public ICollection<SubscriberApplicationEvent> SubscriberApplicationEvents { get; private set; }
        public ICollection<ApplicationEventChannel> ApplicationEventChannels { get; private set; }

        private readonly List<ApplicationEventParameter> _applicationEventParameters;
        public IReadOnlyCollection<ApplicationEventParameter> ApplicationEventParameters => _applicationEventParameters;

        private readonly List<SubscriberFilter> _subscriberFilters;
        public IReadOnlyCollection<SubscriberFilter> SubscriberFilters => _subscriberFilters;

        public void SetNotificationTemplate(Guid notificationTemplateId)
        {
            NotificationTemplateId = notificationTemplateId;
        }

        // DDD Patterns comment
        // This ApplicationEvent  method "AddSubscriberFilter()" should be the only way to add SubscriberFilter to the ApplicationEvent,
        // so any behavior  and validations are controlled by the AggregateRoot
        // in order to maintain consistency between the whole Aggregate.
        public void AddSubscriberFilter(SubscriberFilter subscriberFilter)
        {
            //  _applicationEventSubscriberFilters.Add(subscriberFilter);

            //AddDomainEvent(new SubscriberFilterAddedToApplicationEventEvent(subscriberFilter.Id, subscriberFilter.FilterType, subscriberFilter.FilterValue));
        }

        public void AddApplicationEventParameter(ApplicationEventParameter applicationEventParameter)
        {
            applicationEventParameter.RelateApplicationEventParameter(Id);
            _applicationEventParameters.Add(applicationEventParameter);           
        }

        private void Validate()
        {
            //Todo: Create generic Validate Class
        }
    }
}