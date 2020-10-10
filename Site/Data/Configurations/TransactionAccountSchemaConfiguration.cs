using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Entities;

namespace site.Data.Configurations
{
    internal class TransactionAccountSchemaConfiguration : IEntityTypeConfiguration<TransactionAccount>
    {
        public void Configure(EntityTypeBuilder<TransactionAccount> builder)
        {
            builder.Property(p => p.AccountId).HasMaxLength(25);
            builder.HasIndex(p => p.AccountId).IsUnique().IsClustered(false);
        }
    }
}
