using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class SubscriberApplicationEventEntityConfiguration : IEntityTypeConfiguration<SubscriberApplicationEvent>
    {
        public void Configure(EntityTypeBuilder<SubscriberApplicationEvent> builder)
        {
            builder.ToTable("SubscriberApplicationEvent");

            builder.Ignore(p => p.Id);
            builder.HasKey(s => new { s.SubscriberPartyId, s.ApplicationEventId });

            builder
               .HasOne(bc => bc.Subscriber)
               .WithMany(b => b.SubscriberApplicationEvents)
               .HasForeignKey(bc => bc.SubscriberPartyId);

            builder
                .HasOne(bc => bc.ApplicationEvent)
                .WithMany(c => c.SubscriberApplicationEvents)
                .HasForeignKey(bc => bc.ApplicationEventId);
        }
    }
}