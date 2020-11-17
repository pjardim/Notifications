using AutoMapper;
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain;
using Subscribing.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Subscribing.Application.Queries
{
    public class SubscriberQueries : ISubscriberQueries
    {
        private readonly ISubscriberRepository _subscriberRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IApplicationEventRepository _applicationEventRepository;
        private readonly ISubscriberApplicationEventRepository _subscriberApplicationEventRepository;
        private readonly ISubscriberGroupSubscriberRepository _subscriberGroupSubscriberRepository;
        private readonly IMapper _mapper;

        public SubscriberQueries(
            ISubscriberApplicationEventRepository subscriberApplicationEventRepository,
            ISubscriberRepository subscriberRepository,
            IChannelRepository channelRepository,
            ISubscriberGroupSubscriberRepository subscriberGroupSubscriberRepository,
        IApplicationEventRepository applicationEventRepository,
            IMapper mapper)
        {
            _subscriberApplicationEventRepository = subscriberApplicationEventRepository;
            _subscriberRepository = subscriberRepository ?? throw new ArgumentNullException(nameof(subscriberRepository));
            _channelRepository = channelRepository ?? throw new ArgumentNullException(nameof(channelRepository));
            _subscriberGroupSubscriberRepository = subscriberGroupSubscriberRepository ?? throw new ArgumentNullException(nameof(subscriberGroupSubscriberRepository));
            _applicationEventRepository = applicationEventRepository ?? throw new ArgumentNullException(nameof(applicationEventRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<SubscriberViewModel> GetSubscriberByNameAsync(string subscriberName)
        {
            return _mapper.Map<SubscriberViewModel>(await _subscriberRepository.GetSubscriberByNameAsync(subscriberName));
        }

        public async Task<SubscriberViewModel> GetSubscriberAsync(string subscriberPartyId)
        {
            return _mapper.Map<SubscriberViewModel>(await _subscriberRepository.GetAsync(x => x.SubscriberPartyId == subscriberPartyId)); ;
        }

        public async Task<IEnumerable<SubscriberViewModel>> GetNotificationSubscribers(Guid applicationEventId)
        {
            var applicationEvent = await _applicationEventRepository.GetApplicationEventWithRelatedAsync(applicationEventId);

            var subscribersViewModel = new List<SubscriberViewModel>();

            foreach (var subscriberFilter in applicationEvent.SubscriberFilters)
            {
                if (subscriberFilter.FilterType == SubscriberFilterType.PartyGroup.Name)
                {
                    subscribersViewModel.AddRange(_mapper.Map<List<SubscriberViewModel>>(
                        await _subscriberRepository.GetAllSubscribersByGroup(subscriberFilter.FilterValue)));
                }
            }

            return subscribersViewModel.GroupBy(x => x.SubscriberPartyId).Select(x => x.First());
        }

        public async Task<SubscriberViewModel> GetSubscriberByPartyIdAsync(string partyId)
        {
            return _mapper.Map<SubscriberViewModel>(await _subscriberRepository.GetById(partyId));
        }

        public async Task<SubscriberViewModel> GetSubscriberByPartyIdWithRelatedAsync(string partyId)
        {
            return _mapper.Map<SubscriberViewModel>(await _subscriberRepository.GetWithRelatedAsync(partyId));
        }

        public async Task<List<string>> IdentifyRecipientSubscribers(string recipientPartyId, Guid applicationEventId)
        {
            var recipientSubscribers = new List<SubscriberViewModel>();

            if (recipientPartyId != null)
                recipientSubscribers.Add(await GetSubscriberByPartyIdWithRelatedAsync(recipientPartyId));
            else
                recipientSubscribers.AddRange(await GetNotificationSubscribers(applicationEventId));

            return recipientSubscribers.Select(x => x.SubscriberPartyId).ToList();
        }

        public Task<SubscriberApplicationEventViewModel> GetSubscribersApplicationEvent(IEnumerable<string> receipentPartyIds, Guid applicationEventId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<SubscriberViewModel>> GetAllSubscribersAsync()
        {
            return _mapper.Map<IEnumerable<SubscriberViewModel>>(await _subscriberRepository.GetAllAsync());
        }

        public async Task<IEnumerable<ChannelViewModel>> GetAllChannelsAsync()
        {
            return _mapper.Map<IEnumerable<ChannelViewModel>>(await _channelRepository.GetAllAsync());
        }

        public async Task<IEnumerable<SubscriberApplicationEventViewModel>> GetSubscribersApplicationEventsByPartyId(string partyId)
        {
            return _mapper.Map<IEnumerable<SubscriberApplicationEventViewModel>>(await _subscriberApplicationEventRepository.GetSubscribersApplicationEventsByPartyId(partyId));
        }

        public async Task<SubscriberApplicationEventViewModel> GetSubscribersApplicationEvent(string partyId, Guid applicationEventId)
        {
            return _mapper.Map<SubscriberApplicationEventViewModel>(await _subscriberApplicationEventRepository.GetSubscribersApplicationEvent(partyId, applicationEventId));
        }

        public IEnumerable<SubscriberGroupViewModel> GetAllSubscriberGroups()
        {
            return _mapper.Map<IEnumerable<SubscriberGroupViewModel>>(SubscriberGroup.List());
        }

        public async Task<IEnumerable<SubscriberGroupSubscriberViewModel>> GetAllSubscriberGroupSubscribersByPartyIdAsync(string partyId)
        {
            return _mapper.Map<IEnumerable<SubscriberGroupSubscriberViewModel>>(await _subscriberGroupSubscriberRepository.GetAllByPartyIdAsync(partyId));
        }
    }
}