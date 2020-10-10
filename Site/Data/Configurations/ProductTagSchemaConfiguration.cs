using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data.Configurations
{
    public class ProductTagSchemaConfiguration : IEntityTypeConfiguration<ProductTag>
    {
        public void Configure(EntityTypeBuilder<ProductTag> builder)
        {
            builder.HasOne(e => e.Product).WithMany(e => e.ProductTags).HasForeignKey(e => e.ProductId);
            builder.HasOne(e => e.Tag).WithMany(e => e.ProductTags).HasForeignKey(e => e.TagId);
        }
    }
}
