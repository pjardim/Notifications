using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Notifications.Core.Mediator;
using Notifications.Core.Messaging;
using Notifications.Core.Messaging.CommonMessages.DomainEvents;
using Notifications.Core.Notifications;
using Notifications.Core.SeedWork;
using Notifying.Infrastructure.EntityConfigurations;
using Subscribing.Domain;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Subscribing.Infrastructure
{
    public class SubscriberContext : DbContext, IUnitOfWork
    {
        private readonly IMediatorHandler _mediatorHandler;

        public SubscriberContext(DbContextOptions<SubscriberContext> options, IMediatorHandler mediatorHandler)
            : base(options)
        {
            //The value will be null when created by SubscriberDbContextFactory to be used with EntityCoreDesign( Add-Migrations and UpdateDatabase or another EF-core command )
            _mediatorHandler = mediatorHandler;
        }

        public DbSet<ApplicationEvent> ApplicationEvents { get; set; }
        public DbSet<ApplicationEventChannel> ApplicationEventChannels { get; set; }
        public DbSet<ApplicationEventParameter> ApplicationEventParameters { get; set; }
        public DbSet<Channel> Channels { get; set; }

        public DbSet<ApplicationEventChannelTemplate> ApplicationEventChannelTemplates { get; set; }

        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<SubscriberApplicationEvent> SubscriberApplicationEvents { get; set; }
        public DbSet<SubscriberFilter> SubscriberFilters { get; set; }
        public DbSet<SubscriberGroup> SubscriberGroups { get; set; }
        public DbSet<SubscriberGroupSubscriber> SubscriberGroupSubscribers { get; set; }

        public DbSet<MailBoxItem> MailBox { get; set; }

        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;

        public bool HasActiveTransaction => _currentTransaction != null;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties()
                  .Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }

            //Prefix Id  with table name + Id
            //Id => SubscriberId
            foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties())
                {
                    if (property.GetColumnName() == "Id")
                    {
                        var collunName = entity.GetDefaultTableName() + property.GetColumnName();
                        property.SetColumnName(collunName);
                    }
                }
            }

            modelBuilder.Ignore<Event>();
            modelBuilder.Ignore<DomainEvent>();

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            modelBuilder.ApplyConfiguration(new ApplicationEventEntityConfiguration());

            //Apply all EntityConfigurations from this assmbly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SubscriberContext).Assembly);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            var list = ChangeTracker.Entries();
            try
            {
                foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("CreatedDate") != null))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedDate").CurrentValue = DateTime.Now;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        entry.Property("CreatedDate").IsModified = false;
                    }
                }

                // Dispatch Domain Events collection.
                // Choices:
                // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including
                // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
                // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions.
                // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers.
                // await _mediatorHandler.DispatchDomainEventsAsync(this);

                // After executing this line all the changes (from the Command Handler and Domain Event Handlers)
                // performed through the DbContext will be committed
                var result = await base.SaveChangesAsync(cancellationToken);
                if (result > 0)
                    await _mediatorHandler.DispatchDomainEventsAsync(this);

                return true;
            }
            catch (Exception ex)
            {
                await _mediatorHandler.PublishNotification(new DomainNotification("Commit", $"We had a problem during saving your data"));
                throw ex;
            }
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }
}