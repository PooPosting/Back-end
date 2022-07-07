using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PicturesAPI.Entities.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasQueryFilter(a => !a.IsDeleted);
        builder.HasIndex(a => a.Nickname);
        builder.HasKey(a => a.Id);

        builder
            .Property(a => a.Nickname)
            .HasMaxLength(25)
            .IsRequired();
        builder
            .Property(a => a.Email)
            .HasMaxLength(100)
            .IsRequired();
        builder
            .Property(a => a.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();
        builder
            .Property(a => a.Verified)
            .HasDefaultValue(false)
            .IsRequired();
        builder
            .Property(a => a.ProfilePicUrl)
            .HasMaxLength(255);
        builder
            .Property(a => a.BackgroundPicUrl)
            .HasMaxLength(255);
        builder
            .Property(a => a.AccountDescription)
            .HasMaxLength(500);
        builder
            .Property(a => a.AccountCreated)
            .HasDefaultValue(DateTime.Now);
        builder
            .Property(a => a.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();
        builder
            .Property(a => a.RoleId)
            .HasDefaultValue(1);
    }
}