using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class ChannelEntityConfiguration : IEntityTypeConfiguration<Channel>
    {
        public void Configure(EntityTypeBuilder<Channel> builder)
        {
            builder.ToTable("Channel");

            builder.HasKey(m => m.Id);

            builder.Property(m => m.ChannelName)
           .HasColumnName("ChannelName")
           .IsRequired();
        }
    }
}