using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Subscribing.Infrastructure.EntityConfigurations
{
    public class ApplicationEventChannelTemplateEntityConfiguration : IEntityTypeConfiguration<ApplicationEventChannelTemplate>
    {
        public void Configure(EntityTypeBuilder<ApplicationEventChannelTemplate> builder)
        {
            builder.ToTable("ApplicationEventChannelTemplate");

            builder.Ignore(m => m.Id);
            builder.HasKey(s => new { s.ChannelId, s.ApplicationEventId });

            builder.Property(m => m.Format)
               .HasColumnName("Format")
               .IsRequired();

            builder.Property(m => m.Encoding)
             .HasColumnName("Encoding")
             .IsRequired();

            builder.Property(m => m.Subject)
             .HasColumnName("Subject")
             .IsRequired();

            builder.Property(m => m.Body)
               .HasColumnName("Body")
               .HasColumnType("nvarchar(max)")
               .IsRequired();

            builder
             .HasOne(bc => bc.ApplicationEvent)
             .WithMany(b => b.ApplicationEventChannelTemplates)
             .HasForeignKey(bc => bc.ApplicationEventId);

            builder
                .HasOne(bc => bc.Channel)
                .WithMany(c => c.ApplicationEventChannelTemplates)
                .HasForeignKey(bc => bc.ChannelId);
        }
    }
}