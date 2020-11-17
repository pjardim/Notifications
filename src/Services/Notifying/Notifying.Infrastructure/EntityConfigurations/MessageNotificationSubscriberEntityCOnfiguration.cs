using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifying.Domain.Models.MessageAgregate;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class MessageNotificationSubscriberEntityConfiguration : IEntityTypeConfiguration<NotificationSubscriber>
    {
        public void Configure(EntityTypeBuilder<NotificationSubscriber> builder)
        {
            builder.ToTable("MessageNotificationSubscriber");

            builder.Ignore(m => m.Id);
            builder.HasKey(s => new { s.PartyId, s.MessageNotificationId });

            builder
             .HasOne(bc => bc.Party)
             .WithMany(b => b.NotificationSubscribers)
             .HasForeignKey(bc => bc.PartyId);

            builder
                .HasOne(bc => bc.MessageNotification)
                .WithMany(c => c.NotificationSubscribers)
                .HasForeignKey(bc => bc.MessageNotificationId);
        }
    }
}