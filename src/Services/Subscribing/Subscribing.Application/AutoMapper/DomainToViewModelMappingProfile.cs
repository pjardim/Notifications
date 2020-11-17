using AutoMapper;
using Subscribing.Application.Queries.ViewModels;
using Subscribing.Domain;

namespace Subscribing.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            //ApplicationEvents Maps
            CreateMap<ApplicationEvent, ApplicationEventViewModel>();
            CreateMap<ApplicationEventParameter, ApplicationEventParameterViewModel>();
            CreateMap<ApplicationEventChannelTemplate, ApplicationEventChannelTemplateViewModel>();
            CreateMap<ApplicationEventChannel, ApplicationEventChannelViewModel>();

            //Boundary Maps
            CreateMap<SubscriberApplicationEvent, SubscriberApplicationEventViewModel>();

            //SubscriberMaps
            CreateMap<Subscriber, SubscriberViewModel>();
            CreateMap<SubscriberFilter, SubscriberFilterViewModel>();
            CreateMap<SubscriberGroup, SubscriberGroupViewModel>();
            CreateMap<SubscriberGroupSubscriber, SubscriberGroupSubscriberViewModel>();
            CreateMap<SubscriberFilterType, SubscriberFilterTypeViewModel>();

            CreateMap<Channel, ChannelViewModel>();
            CreateMap<MailBoxItem, MailBoxItemViewModel>()
                .ForMember(dest => dest.Senders, act => act.Ignore());
        }
    }
}