using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Notifications.Core.Mediator;
using Subscribing.Infrastructure;
using System.IO;

namespace WebMVC.Infrastructure.Factories
{
    public class SubscriberDbContextFactory : IDesignTimeDbContextFactory<SubscriberContext>
    {
        private readonly IMediatorHandler _mediatorHandler;

        //This Method will be called outside from scope application from EntityFrameworkCoreDesign to Add-Migrations or Update-Database
        //Mediator Dependence will be not add to the ServicesColletion,so it will be not used in the SubscriberContext 
        //The _mediatorHandler will be aways null in that case.
        public SubscriberDbContextFactory()
        {
        }

        public SubscriberDbContextFactory(
            IMediatorHandler mediatorHandler
            )
        {
            _mediatorHandler = mediatorHandler;
        }

        public SubscriberContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SubscriberContext>();

            optionsBuilder.UseSqlServer(config["ConnectionString"], sqlServerOptionsAction: o => o.MigrationsAssembly("WebMVC"));

            return new SubscriberContext(optionsBuilder.Options, _mediatorHandler);
        }
    }
}