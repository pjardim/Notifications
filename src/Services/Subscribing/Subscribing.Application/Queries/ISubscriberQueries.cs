using Subscribing.Application.Queries.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Subscribing.Application.Queries
{
    public interface ISubscriberQueries
    {
        Task<SubscriberViewModel> GetSubscriberByNameAsync(string subscriberName);

        Task<SubscriberViewModel> GetSubscriberByPartyIdAsync(string recipientPartyId);

        Task<SubscriberViewModel> GetSubscriberByPartyIdWithRelatedAsync(string recipientPartyId);

        Task<List<string>> IdentifyRecipientSubscribers(string payload, Guid applicationEventId);

        Task<IEnumerable<SubscriberViewModel>> GetAllSubscribersAsync();

        Task<IEnumerable<ChannelViewModel>> GetAllChannelsAsync();

        Task<IEnumerable<SubscriberApplicationEventViewModel>> GetSubscribersApplicationEventsByPartyId(string partyId);

        Task<SubscriberApplicationEventViewModel> GetSubscribersApplicationEvent(string partyId, Guid id);

        IEnumerable<SubscriberGroupViewModel> GetAllSubscriberGroups();
        Task<IEnumerable<SubscriberGroupSubscriberViewModel>> GetAllSubscriberGroupSubscribersByPartyIdAsync(string partyId);
    }
}