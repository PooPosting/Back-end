using Microsoft.EntityFrameworkCore;
using PicturesAPI.Entities.Joins;

namespace PicturesAPI.Entities;

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

    // many-to-many joins
    public virtual DbSet<PictureTagJoin> PictureTagJoins { get; set; }
    public virtual DbSet<PictureSeenByAccountJoin> PictureAccountJoins { get; set; }
    public virtual DbSet<AccountLikedTagJoin> AccountLikedTagJoins { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PictureTagJoin>()
            .HasOne(p => p.Picture)
            .WithMany(t => t.PictureTagJoins)
            .HasForeignKey(pid => pid.PictureId);

        modelBuilder.Entity<PictureTagJoin>()
            .HasOne(p => p.Tag)
            .WithMany(t => t.PictureTagJoins)
            .HasForeignKey(tid => tid.TagId);

        modelBuilder.Entity<PictureSeenByAccountJoin>()
            .HasOne(p => p.Picture)
            .WithMany(a => a.PictureAccountJoins)
            .HasForeignKey(aid => aid.AccountId);

        modelBuilder.Entity<PictureSeenByAccountJoin>()
            .HasOne(p => p.Account)
            .WithMany(a => a.PictureAccountJoins)
            .HasForeignKey(pid => pid.PictureId);

        modelBuilder.Entity<AccountLikedTagJoin>()
            .HasOne(a => a.Account)
            .WithMany(atj => atj.AccountLikedTagJoins)
            .HasForeignKey(aid => aid.AccountId);

        modelBuilder.Entity<AccountLikedTagJoin>()
            .HasOne(a => a.Tag)
            .WithMany(atj => atj.AccountLikedTagJoins)
            .HasForeignKey(aid => aid.TagId);
    }
}