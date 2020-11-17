using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Notifying.Domain.Models.MessageAgregate;

namespace Notifying.Infrastructure.EntityConfigurations
{
    internal class PartyEntityConfiguration : IEntityTypeConfiguration<Party>
    {
        public void Configure(EntityTypeBuilder<Party> builder)
        {
            builder.ToTable("Party");

            builder.Ignore(o => o.Id);
            builder.HasKey(o => o.PartyId);
        }
    }
}