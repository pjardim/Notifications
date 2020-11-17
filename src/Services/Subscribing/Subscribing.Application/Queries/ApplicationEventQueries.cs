using AutoMapper;
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Application.Queries
{
    public class ApplicationEventQueries : IApplicationEventQueries
    {
        private readonly IApplicationEventRepository _applicationEventRepository;
        private readonly ISubscriberApplicationEventRepository _subscriberApplicationEventRepository;
        private readonly IApplicationEventParameterRepository _applicationEventParameterRepository;
        private readonly ISubscriberFilterRepository _subscriberFilterRepository;
        private readonly IApplicationEventChannelTemplateRepository _applicationEventChannelTemplateRepository;
        private readonly IMapper _mapper;

        public ApplicationEventQueries(
            IApplicationEventRepository applicationEventRepository,
            ISubscriberFilterRepository subscriberFilterRepository,
            ISubscriberApplicationEventRepository subscriberApplicationEventRepository,
            IApplicationEventParameterRepository applicationEventParameterRepository,
            IApplicationEventChannelTemplateRepository applicationEventChannelTemplateRepository,
            IMapper mapper)
        {
            _applicationEventRepository = applicationEventRepository ?? throw new ArgumentNullException(nameof(applicationEventRepository));
            _subscriberFilterRepository = subscriberFilterRepository ?? throw new ArgumentNullException(nameof(subscriberFilterRepository));
            _subscriberApplicationEventRepository = subscriberApplicationEventRepository ?? throw new ArgumentNullException(nameof(applicationEventRepository));
            _applicationEventParameterRepository = applicationEventParameterRepository ?? throw new ArgumentNullException(nameof(applicationEventParameterRepository));
            _applicationEventChannelTemplateRepository = applicationEventChannelTemplateRepository ?? throw new ArgumentNullException(nameof(applicationEventChannelTemplateRepository)); ;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ApplicationEventViewModel>> GetAllApplicationEventsAsync()
        {
            return _mapper.Map<IEnumerable<ApplicationEventViewModel>>(await _applicationEventRepository.GetAllAsync());
        }

        public async Task<ApplicationEventViewModel> GetApplicationEventByIdAsync(Guid applicationEventId)
        {
            return _mapper.Map<ApplicationEventViewModel>(await _applicationEventRepository.GetApplicationEventWithRelatedAsync(applicationEventId));
        }

        public async Task<ApplicationEventViewModel> GetApplicationEventByNameAsync(string applicationEventName)
        {
            return _mapper.Map<ApplicationEventViewModel>(await _applicationEventRepository.GetApplicationEventByNameAsync(applicationEventName));
        }

        public async Task<IEnumerable<ApplicationEventViewModel>> GetAllApplicationEventsWithRelatedAsync()
        {
            return _mapper.Map<IEnumerable<ApplicationEventViewModel>>(await _applicationEventRepository.GetAllApplicationEventsWithRelatedAsync());
        }

        public async Task<ApplicationEventViewModel> GetApplicationEventWithRelatedAsync(Guid applicationEventId)
        {
            return _mapper.Map<ApplicationEventViewModel>(await _applicationEventRepository.GetApplicationEventWithRelatedAsync(applicationEventId));
        }

        public async Task<IEnumerable<ApplicationEventParameterViewModel>> GetAllApplicationParametersByApplicationEventIdAsync(Guid applicationEventId)
        {
            return _mapper.Map<IEnumerable<ApplicationEventParameterViewModel>>(await _applicationEventParameterRepository.GetByApplicationEventIdAsync(applicationEventId));
        }

        public async Task<ApplicationEventParameterViewModel> GetApplicationEventParameterById(Guid applicationEventParameterId)
        {
            return _mapper.Map<ApplicationEventParameterViewModel>(await _applicationEventParameterRepository.GetByIdWithRelatedAsync(applicationEventParameterId));
        }

        public async Task<ApplicationEventChannelTemplateViewModel> GetNotificationTemplateData(Guid applicationEventId, string messageChannel)
        {
            return _mapper.Map<ApplicationEventChannelTemplateViewModel>(await _applicationEventChannelTemplateRepository.GetByApplicationEventAndChannel(applicationEventId, messageChannel));
        }

        public async Task<IEnumerable<SubscriberFilterViewModel>> GetAllSubscriberFiltersAsync()
        {
            return _mapper.Map<IEnumerable<SubscriberFilterViewModel>>(await _subscriberFilterRepository.GetAllAsync());
        }

        public IEnumerable<SubscriberFilterTypeViewModel> GetAllSubscriberFilterTypes()
        {
            return _mapper.Map<IEnumerable<SubscriberFilterTypeViewModel>>(_subscriberFilterRepository.GetAllSubscriberFilterTypes());
        }
    }
}