using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifying.Domain.Models.MessageAgregate;

namespace Notifying.Infrastructure.EntityConfigurations
{
    public class MessageNotificationEntityConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("MessageNotification");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.PayLoad)
                .HasColumnName("PayLoad")
                .HasColumnType("varchar(max)")
                .IsRequired();

            builder.Property(m => m.ApplicationEventId)
            .HasColumnName("ApplicationEventId")
            .IsRequired();

            builder.HasOne(m => m.Message)
               .WithMany(n => n.Notifications)
               .HasForeignKey(bc => bc.MessageId);

            builder
              .HasOne(bc => bc.PublisherParty)
              .WithMany(b => b.Notifications)
              .HasForeignKey(bc => bc.PublisherPartyId);
        }
    }
}