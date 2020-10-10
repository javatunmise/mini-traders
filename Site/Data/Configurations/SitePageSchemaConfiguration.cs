using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace site.Data.Configurations
{
    internal class SitePageSchemaConfiguration : IEntityTypeConfiguration<SitePage>
    {
        public void Configure(EntityTypeBuilder<SitePage> builder)
        {
            builder.Property(p => p.Title).HasMaxLength(128);
            builder.Property(p => p.Content).HasMaxLength(8000);
        }
    }

}
