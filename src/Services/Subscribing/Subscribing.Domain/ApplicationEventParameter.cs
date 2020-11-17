using Notifications.Core.SeedWork;
using System;

namespace Subscribing.Domain
{
    public class ApplicationEventParameter : Entity, IAggregateRoot
    {
        protected ApplicationEventParameter()
        {
        }

        public ApplicationEventParameter(Guid id, Guid applicationEventId, string parameterName, string description) : this()
        {
            Id = id;
            ApplicationEventId = applicationEventId;
            ParameterName = parameterName;
            Description = description;
        }

        public Guid ApplicationEventId { get; private set; }
        public ApplicationEvent ApplicationEvent { get; private set; }
        public string ParameterName { get; private set; }
        public string Description { get; private set; }

        internal void RelateApplicationEventParameter(Guid id)
        {
            ApplicationEventId = id;
        }
    }
}