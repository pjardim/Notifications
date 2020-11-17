using Autofac;
using EventSourcing;
using MediatR;
using Notifications.Core.Data.EventSourcing;
using Subscribing.Application.Queries;
using Subscribing.Domain.Interfaces.Repositories;
using Subscribing.Infrastructure.Repository;

namespace Notifying.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
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


            //EventSource
            builder.RegisterType<EventSourcingRepository>().As<IEventSourcingRepository>().InstancePerLifetimeScope();

            //EventStore
            builder.RegisterType<EventStoreService>().As<IEventStoreService>().InstancePerLifetimeScope();

            //Integration Queries
            builder.RegisterType<ApplicationEventQueries>().As<IApplicationEventQueries>().InstancePerLifetimeScope();
            builder.RegisterType<SubscriberQueries>().As<ISubscriberQueries>().InstancePerLifetimeScope();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });
        }
    }
}