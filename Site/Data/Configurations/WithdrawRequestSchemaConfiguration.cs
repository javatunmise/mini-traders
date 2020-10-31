using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace site.Data.Configurations
{
    public class WithdrawRequestSchemaConfiguration : IEntityTypeConfiguration<WithdrawRequest>
    {
        public void Configure(EntityTypeBuilder<WithdrawRequest> builder)
        {
            builder.Property(p => p.Amount).HasColumnType("decimal(18,2)");
            builder.Property(p => p.Charge).HasColumnType("decimal(18,2)");
            builder.Property(p => p.WalletCode).HasMaxLength(30);
            builder.Property(p => p.AccountNumber).HasMaxLength(20);
            builder.Property(p => p.BankName).HasMaxLength(50);
        }
    }
}
