using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;
using System.Collections.Generic;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class SubscriberEntityConfiguration : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.ToTable("Subscriber");

            builder.Ignore(x => x.Id);
            builder.HasKey(s => s.SubscriberPartyId);

            builder.Property(m => m.Name)
                .HasColumnName("Name")
                .IsRequired();

            builder.Property(m => m.Email)
             .HasColumnName("Email")
             .IsRequired();

    }
}
}