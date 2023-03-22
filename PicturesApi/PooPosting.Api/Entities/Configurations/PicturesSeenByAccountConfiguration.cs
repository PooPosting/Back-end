using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PooPosting.Api.Entities.Joins;

namespace PooPosting.Api.Entities.Configurations;

public class PicturesSeenByAccountConfiguration: IEntityTypeConfiguration<PictureSeenByAccount>
{
    public void Configure(EntityTypeBuilder<PictureSeenByAccount> builder)
    {
        builder.HasQueryFilter(p => !p.Account.IsDeleted);
        builder.HasKey(p => p.Id);

        builder
            .HasOne(p => p.Picture)
            .WithMany(a => a.SeenByAccount);
        builder
            .HasOne(p => p.Account)
            .WithMany(a => a.PicturesSeen);
    }
}