using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace site.Data.Configurations
{
    public class ProductReviewSchemaConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.Property(p => p.Message).HasMaxLength(1024);
            builder.Property(p => p.ReviewerName).HasMaxLength(100);

            builder.HasIndex(p => new { p.ProductId, p.StoreId }).HasName("IX_Product_And_Store");
        }
    }
}