using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PooPosting.Api.Entities.Configurations;

public class LikeConfiguration: IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.HasQueryFilter(l => !l.Account.IsDeleted);
        builder.HasKey(c => c.Id);
        
        builder
            .Property(p => p.Liked)
            .HasDefaultValueSql("now()")
            .IsRequired();

    }
}