using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace site.Data.Configurations
{
    internal class FlashDealsSchemaConfiguration : IEntityTypeConfiguration<FlashDeal>
    {
        public void Configure(EntityTypeBuilder<FlashDeal> builder)
        {
            builder.Property(x => x.MinimumDiscount).HasColumnType("decimal(18,2)");
        }
    }
    internal class FlashDealProductsSchemaConfiguration : IEntityTypeConfiguration<FlashDealProduct>
    {
        public void Configure(EntityTypeBuilder<FlashDealProduct> builder)
        {
            builder.HasKey(x => new { x.ProductId, x.FlashDealId });
            builder.Property(x => x.OldPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.CurrentPrice).HasColumnType("decimal(18,2)");
            builder.Property(x => x.Discount).HasColumnType("decimal(18,2)");
        }
    }
}
