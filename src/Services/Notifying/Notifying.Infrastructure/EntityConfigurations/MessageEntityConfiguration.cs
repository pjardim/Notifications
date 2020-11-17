using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifying.Domain.Models.MessageAgregate;

namespace Notifying.Infrastructure.EntityConfigurations
{
    public class MessageEntityConfiguration : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.ToTable("Message");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.MessageChannel)
              .HasColumnName("MessageChannel")
              .IsRequired();

            builder.Property(m => m.CreatedDate)
            .HasColumnName("CreatedDate")
            .IsRequired();

            builder.HasMany(m => m.Notifications)
                .WithOne(n => n.Message)
                .HasForeignKey(c => c.Id);

            builder.HasOne(o => o.MessageStatus)
               .WithMany()
               .HasForeignKey("_messageStatusId");

            builder
               .Property<int>("_messageStatusId")
               .UsePropertyAccessMode(PropertyAccessMode.Field)
               .HasColumnName("MessageStatusId")
               .IsRequired();
        }
    }
}