using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Subscribing.Infrastructure.EntityConfigurations
{
    class SubscriberGroupEntityConfiguration : IEntityTypeConfiguration<SubscriberGroup>
    {
        public void Configure(EntityTypeBuilder<SubscriberGroup> builder)
        {
            builder.ToTable("SubscriberGroup");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}