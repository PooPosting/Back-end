using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PicturesAPI.Entities.Configurations;

public class TagConfiguration: IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.HasKey(t => t.Id);

        builder
            .Property(t => t.Value)
            .HasMaxLength(25);
    }
}