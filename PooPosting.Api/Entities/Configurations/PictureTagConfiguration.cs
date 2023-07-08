using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PooPosting.Api.Entities.Joins;

namespace PooPosting.Api.Entities.Configurations;

public class PictureTagConfiguration: IEntityTypeConfiguration<PictureTag>
{
    public void Configure(EntityTypeBuilder<PictureTag> builder)
    {
        builder.HasKey(p => p.Id);
        builder.HasQueryFilter(p => !p.Picture.IsDeleted);

        builder
            .HasOne(p => p.Picture)
            .WithMany(t => t.PictureTags);
        builder
            .HasOne(p => p.Tag)
            .WithMany(t => t.PictureTags);
    }
}