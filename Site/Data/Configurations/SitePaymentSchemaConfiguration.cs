using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace site.Data.Configurations
{
    internal class SitePaymentSchemaConfiguration : IEntityTypeConfiguration<SitePayment>
    {
        public void Configure(EntityTypeBuilder<SitePayment> builder)
        {
            builder.Property(p => p.PaymentRef).HasMaxLength(20);
            builder.Property(p => p.PaymentDescription).HasMaxLength(128);
            builder.Property(p => p.EntityType).HasMaxLength(20);
            builder.Property(p => p.EntityId).HasMaxLength(50);
            builder.Property(p => p.Email).HasMaxLength(128);
            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.ExternalRef).HasMaxLength(128);
            builder.HasKey(p => p.PaymentRef);
        }
    }

}
