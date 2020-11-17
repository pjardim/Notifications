using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Subscribing.Domain;

namespace Subscribing.Infrastructure.EntityConfigurations
{
    internal class MailBoxEntityConfiguration : IEntityTypeConfiguration<MailBoxItem>
    {
        public void Configure(EntityTypeBuilder<MailBoxItem> builder)
        {
            builder.ToTable("MailBoxItem");

            //the Message is unique for each recipient
            builder.Ignore(m => m.Id);
            builder.HasKey(m => new { m.MessageId, m.RecipientPartyId });

            builder.Property(m => m.Body)
           .HasColumnName("Body")
           .HasColumnType("varchar(max)")
           .IsRequired();

            builder
            .HasOne(d => d.Recipient)
            .WithMany(x => x.RecipientMailBoxItems)
            .HasForeignKey(d => d.RecipientPartyId);
        }
    }
}