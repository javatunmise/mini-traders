using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data.Configurations
{
    internal class SiteUserSchemaConfiguration : IEntityTypeConfiguration<SiteUser>
    {
        public void Configure(EntityTypeBuilder<SiteUser> builder)
        {
            builder.Property(p => p.FirstName).HasMaxLength(50);
            builder.Property(p => p.LastName).HasMaxLength(50);
            builder.Property(p => p.PhoneNumber).HasMaxLength(20);
            builder.Property(p => p.Email).HasMaxLength(255);
            builder.Property(p => p.ProfilePicturePath).HasMaxLength(255);

            builder.HasOne(user => user.Store)
                   .WithOne(store => store.User)
                   .HasForeignKey<Store>(store => store.SiteUserId);
        }
    }

}
