using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace site.Data.Configurations
{
    internal class SiteSchemaConfiguration : IEntityTypeConfiguration<Shared.Entities.Site>
    {
        public void Configure(EntityTypeBuilder<Shared.Entities.Site> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.AboutUsIntro).HasMaxLength(1024);
            builder.Property(p => p.GoogleAnalyticsScript).HasMaxLength(1024);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
            builder.Property(p => p.Email).HasMaxLength(128);
            builder.Property(p => p.Address).HasMaxLength(128);
            builder.Property(p => p.CopyRightName).HasMaxLength(50);
            builder.Property(p => p.WorkingHours).HasMaxLength(20);
            builder.Property(p => p.FacebookUrl).HasMaxLength(128);
            builder.Property(p => p.TwitterUrl).HasMaxLength(128);
            builder.Property(p => p.InstagramUrl).HasMaxLength(128);
            builder.Property(p => p.LinkedInUrl).HasMaxLength(128);
            builder.Property(p => p.YoutubeUrl).HasMaxLength(128);
        }
    }

}
