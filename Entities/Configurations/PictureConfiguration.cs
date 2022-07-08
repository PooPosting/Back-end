using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PicturesAPI.Entities.Configurations;

public class PictureConfiguration: IEntityTypeConfiguration<Picture>
{
    public void Configure(EntityTypeBuilder<Picture> builder)
    {
        builder.HasQueryFilter(p => !p.IsDeleted);
        builder.HasIndex(p => p.PopularityScore);
        builder.HasIndex(p => p.Name);
        builder.HasKey(p => p.Id);

        builder
            .Property(p => p.Name)
            .HasMaxLength(40)
            .IsRequired();
        builder
            .Property(p => p.Description)
            .HasMaxLength(500);
        builder
            .Property(p => p.Url)
            .HasMaxLength(250)
            .IsRequired();
        builder
            .Property(p => p.PictureAdded)
            .HasDefaultValueSql("now()")
            .IsRequired();
        builder
            .Property(p => p.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
        builder
            .Property(p => p.PopularityScore)
            .HasDefaultValue(36500)
            .IsRequired();
    }
}