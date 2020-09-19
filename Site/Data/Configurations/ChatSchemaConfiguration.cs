using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data.Configurations
{
    public class ChatSchemaConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.Property(p => p.Subject).HasMaxLength(225);
            builder.HasOne(p => p.Initiator).WithMany().HasForeignKey(p => p.InitiatorUserId);
            builder.HasOne(p => p.Recipient).WithMany().HasForeignKey(p => p.RecipientUserId);
        }
    }
}
