using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class ApplicationEventChannelEntityConfiguration : IEntityTypeConfiguration<ApplicationEventChannel>
    {
        public void Configure(EntityTypeBuilder<ApplicationEventChannel> builder)
        {
            builder.ToTable("ApplicationEventChannel");

            builder.Ignore(m => m.Id);
            builder.HasKey(s => new { s.ChannelId, s.ApplicationEventId });

            builder.Property(m => m.DelayedSendMinutes)
               .HasColumnName("DelayedSendMinutes")
               .IsRequired();

            builder.Property(m => m.Enabled)
               .HasColumnName("Enabled")
               .IsRequired();


            builder
             .HasOne(bc => bc.ApplicationEvent)
             .WithMany(b => b.ApplicationEventChannels)
             .HasForeignKey(bc => bc.ApplicationEventId);

            builder
                .HasOne(bc => bc.Channel)
                .WithMany(c => c.ApplicationEventChannels)
                .HasForeignKey(bc => bc.ChannelId);


        }
    }
}