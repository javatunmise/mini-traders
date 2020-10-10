using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data.Configurations
{
    public class CategorySchemaConfiguration : IEntityTypeConfiguration<Shared.Entities.Category>
    {
        public void Configure(EntityTypeBuilder<Shared.Entities.Category> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(100);
            builder.Property(p => p.IconImagePath).HasMaxLength(128);
        }
    }
}
