using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Subscribing.Infrastructure.EntityConfigurations
{
    public class SubscribeGroupSubscriberEntityConfiguration : IEntityTypeConfiguration<SubscriberGroupSubscriber>
    {
        public void Configure(EntityTypeBuilder<SubscriberGroupSubscriber> builder)
        {
            builder.ToTable("SubscriberGroupSubscriber");

            builder.Ignore(s => s.Id);
            builder.HasKey(s => new { s.SubscriberPartyId, s.SubscriberGroupId });

            builder.HasOne(s => s.SubscriberGroup)
                .WithMany()
                .HasForeignKey("SubscriberGroupId");

            builder
                .HasOne(s => s.Subscriber)
                .WithMany(s => s.SubscriberGroupSubscriber)
                .HasForeignKey(s => s.SubscriberPartyId);
        }
    }
}