using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PooPosting.Data.DbContext.Entities.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);
        builder.HasIndex(a => a.Nickname);
        builder.HasKey(a => a.Id);
    }
}