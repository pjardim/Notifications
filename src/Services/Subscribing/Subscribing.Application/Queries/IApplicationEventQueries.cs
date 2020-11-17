
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Application.Queries
{
    public interface IApplicationEventQueries
    {
        Task<ApplicationEventViewModel> GetApplicationEventByIdAsync(Guid id);
        Task<IEnumerable<ApplicationEventViewModel>> GetAllApplicationEventsAsync();
        Task<ApplicationEventViewModel> GetApplicationEventByNameAsync(string applicationEventName);
        Task<IEnumerable<ApplicationEventViewModel>> GetAllApplicationEventsWithRelatedAsync();
        Task<ApplicationEventViewModel> GetApplicationEventWithRelatedAsync(Guid applicationEventId);
        Task<IEnumerable<ApplicationEventParameterViewModel>> GetAllApplicationParametersByApplicationEventIdAsync(Guid applicationEventId);
        Task<ApplicationEventParameterViewModel> GetApplicationEventParameterById(Guid applicationEventParameterId);
        Task<ApplicationEventChannelTemplateViewModel> GetNotificationTemplateData(Guid applicationEventId, string messageChannel);
        Task<IEnumerable<SubscriberFilterViewModel>> GetAllSubscriberFiltersAsync();
        IEnumerable<SubscriberFilterTypeViewModel> GetAllSubscriberFilterTypes();
    }
}