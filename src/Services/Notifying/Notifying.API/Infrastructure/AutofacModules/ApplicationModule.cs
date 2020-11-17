using Autofac;
using EventBus.Abstractions;
using EventSourcing;
using Notifications.Core.Data.EventSourcing;
using Notifying.API.Application.CommandHandlers;
using Notifying.API.Infrastructure.Repositories;
using Subscribing.Application.Queries;
using Subscribing.Domain.Interfaces.Repositories;
using Subscribing.Infrastructure.Repository;
using System.Reflection;

namespace Notifying.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationsNoSqlContext>();
            builder.RegisterType<NotificationsNoSqlRepository>().As<INotificationsNoSqlRepository>().InstancePerLifetimeScope();

            //Todo use SignalR to communicate with anothers Microservices and  remove  This dependences
            //Repositories
            builder.RegisterType<ApplicationEventRepository>().As<IApplicationEventRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationEventChannelRepository>().As<IApplicationEventChannelRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationEventChannelTemplateRepository>().As<IApplicationEventChannelTemplateRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ApplicationEventParameterrepository>().As<IApplicationEventParameterRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberApplicationEventRepository>().As<ISubscriberApplicationEventRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberRepository>().As<ISubscriberRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberFilterRepository>().As<ISubscriberFilterRepository>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberGroupSubscriberRepository>().As<ISubscriberGroupSubscriberRepository>().InstancePerLifetimeScope();

            builder.RegisterType<ChannelRepository>().As<IChannelRepository>().InstancePerLifetimeScope();
            builder.RegisterType<MailBoxRepository>().As<IMailBoxRepository>().InstancePerLifetimeScope();




            builder.RegisterAssemblyTypes(typeof(MessageCommandHandler).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(IIntegrationEventHandler<>));

            builder.RegisterType<ApplicationEventRepository>().As<IApplicationEventRepository>().InstancePerLifetimeScope();

            builder.RegisterType<ApplicationEventQueries>().As<IApplicationEventQueries>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberQueries>().As<ISubscriberQueries>().InstancePerLifetimeScope();

            // Event Sourcing
            builder.RegisterType<EventStoreService>().As<IEventStoreService>().InstancePerLifetimeScope();
            builder.RegisterType<EventSourcingRepository>().As<IEventSourcingRepository>().InstancePerLifetimeScope();
        }
    }
}