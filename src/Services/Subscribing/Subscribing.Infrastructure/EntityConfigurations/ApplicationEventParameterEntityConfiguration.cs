using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class ApplicationEventParameterEntityConfiguration : IEntityTypeConfiguration<ApplicationEventParameter>
    {
        public void Configure(EntityTypeBuilder<ApplicationEventParameter> builder)
        {
            builder.ToTable("ApplicationEventParameter");

            builder.Property(m => m.ParameterName)
               .HasColumnName("ParameterName")
               .IsRequired();

            builder.Property(m => m.Description)
               .HasColumnName("Description")
               .IsRequired();
        }
    }
}