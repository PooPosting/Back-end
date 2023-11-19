using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PooPosting.Api.Entities.Configurations;

public class CommentConfiguration: IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted && !c.Account.IsDeleted);
        builder.HasIndex(c => c.CommentAdded);
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