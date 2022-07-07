using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PicturesAPI.Entities.Configurations;

public class RestrictedIpConfiguration: IEntityTypeConfiguration<RestrictedIp>
{
    public void Configure(EntityTypeBuilder<RestrictedIp> builder)
    {
        builder.HasKey(i => i.Id);

        builder
            .Property(i => i.IpAddress)
            .HasMaxLength(256)
            .IsRequired();
        builder
            .Property(i => i.CantGet)
            .IsRequired();
        builder
            .Property(i => i.CantPost)
            .IsRequired();
    }
}