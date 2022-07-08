using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PicturesAPI.Entities.Configurations;

public class CommentConfiguration: IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);
        builder.HasKey(c => c.Id);

        builder
            .Property(c => c.Text)
            .HasMaxLength(500)
            .IsRequired();
        builder
            .Property(c => c.CommentAdded)
            .HasDefaultValueSql("now()")
            .IsRequired();
        builder
            .Property(c => c.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
    }
}