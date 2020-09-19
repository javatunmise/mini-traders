using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace site.Data.Configurations
{
    public class SiteImageSchemaConfiguration : IEntityTypeConfiguration<SiteImage>
    {
        public void Configure(EntityTypeBuilder<SiteImage> builder)
        {
            builder.Property(p => p.ImagePath).HasMaxLength(128);
            builder.Property(p => p.Link).HasMaxLength(128);
            builder.Property(p => p.EntityId).HasMaxLength(50);
        }
    }
}
