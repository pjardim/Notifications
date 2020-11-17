using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class ApplicationEventEntityConfiguration : IEntityTypeConfiguration<ApplicationEvent>
    {
        public void Configure(EntityTypeBuilder<ApplicationEvent> builder)
        {
            builder.ToTable("ApplicationEvent");

            builder.HasKey(m => m.Id);

            builder
                 .HasMany(c => c.ApplicationEventParameters)
                 .WithOne(e => e.ApplicationEvent);
        }
    }
}