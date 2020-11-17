using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class SubscriberFilterEntityConfiguration : IEntityTypeConfiguration<SubscriberFilter>
    {
        public void Configure(EntityTypeBuilder<SubscriberFilter> builder)
        {
            builder.ToTable("SubscriberFilter");

            builder.HasKey(o => o.Id);
        }
    }
}