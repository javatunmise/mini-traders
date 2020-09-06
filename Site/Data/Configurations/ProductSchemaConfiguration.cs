using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Shared.Entities;
using System.Collections.Generic;

namespace site.Data.Configurations
{
    internal class ProductSchemaConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(128);
            builder.Property(p => p.ProductDetails).HasMaxLength(512);
            builder.Property(p => p.Specifications).HasMaxLength(1024);
            builder.Property(p => p.ImageUrl).HasMaxLength(255);
            builder.Property(p => p.SmallImageUrl).HasMaxLength(255);
            builder.Property(p => p.OtherImageUrlsJson).HasMaxLength(1024);
            builder.Property(p => p.Price).HasColumnType("decimal(18,2)");
            builder.Property(p => p.OldPrice).HasColumnType("decimal(18,2)");
            builder.Property(p => p.AverageRating).HasColumnType("decimal(18,2)");
        }
    }

}
