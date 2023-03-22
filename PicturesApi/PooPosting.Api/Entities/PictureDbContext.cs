using Microsoft.EntityFrameworkCore;
using PooPosting.Api.Entities.Joins;

namespace PooPosting.Api.Entities;

public class PictureDbContext : DbContext
{
    public PictureDbContext(DbContextOptions<PictureDbContext> options) : base(options) { }
        
    public virtual DbSet<Picture> Pictures { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Like> Likes { get; set; }
    public virtual DbSet<Comment> Comments { get; set; }
    public virtual DbSet<RestrictedIp> RestrictedIps { get; set; }
    public virtual DbSet<Tag> Tags { get; set; }

    // many-to-many
    public virtual DbSet<PictureTag> PictureTags { get; set; }
    public virtual DbSet<PictureSeenByAccount> PicturesSeenByAccounts { get; set; }
    public virtual DbSet<AccountLikedTag> AccountsLikedTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PictureDbContext).Assembly);
    }
}