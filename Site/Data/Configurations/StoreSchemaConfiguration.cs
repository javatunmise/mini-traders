using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using System.Net.NetworkInformation;

namespace site.Data.Configurations
{
    internal class StoreSchemaConfiguration : IEntityTypeConfiguration<Store>
    {
        public void Configure(EntityTypeBuilder<Store> builder)
        {
            //builder.Property(p => p.Id).HasMaxLength(50);
            builder.Property(p => p.StoreName).HasMaxLength(100);
            builder.Property(p => p.StoreDescription).HasMaxLength(1024);
            builder.Property(p => p.LogoPath).HasMaxLength(255);
            builder.Property(p => p.UploadedDocPath).HasMaxLength(255);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
            builder.Property(p => p.ReferrerCode).HasMaxLength(20);

            builder.HasOne(p => p.Hostel).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(p => p.Campus).WithMany().OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.LastSubscriptionAmount).HasColumnType("decimal(18,2)");
        }
    }

}
