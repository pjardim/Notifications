using EventSourcing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Core.Data.EventSourcing;
using Notifications.Core.Notifications;
using Subscribing.Application.CommandHandlers;
using Subscribing.Application.Commands;

namespace WebMVC.Setup
{
    public static class DependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            // Notifications
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<IRequestHandler<CreateApplicationEventCommand, bool>, ApplicationEventCommandHandler>();

            // Event Sourcing
            services.AddSingleton<IEventStoreService, EventStoreService>();
            services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();
        }
    }
}