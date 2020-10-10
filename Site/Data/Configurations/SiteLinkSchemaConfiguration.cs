using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace site.Data.Configurations
{
    internal class SiteLinkSchemaConfiguration : IEntityTypeConfiguration<SiteLink>
    {
        public void Configure(EntityTypeBuilder<SiteLink> builder)
        {
            builder.Property(p => p.Text).HasMaxLength(50);
            builder.Property(p => p.Url).HasMaxLength(128);
            builder.Property(p => p.Target).HasMaxLength(10);
        }
    }

}
