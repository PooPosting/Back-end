using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PooPosting.Data.DbContext.Entities.Configurations;

public class PictureConfiguration: IEntityTypeConfiguration<Picture>
{
    public void Configure(EntityTypeBuilder<Picture> builder)
    {
        builder.HasQueryFilter(p => !p.IsDeleted && !p.Account.IsDeleted);
        builder.HasIndex(p => p.PopularityScore);
        builder.HasIndex(p => p.PictureAdded);
        builder.HasKey(p => p.Id);
    }
}