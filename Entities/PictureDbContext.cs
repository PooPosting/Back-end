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

    // many-to-many
    public virtual DbSet<PictureTag> PictureTags { get; set; }
    public virtual DbSet<PictureSeenByAccount> PicturesSeenByAccounts { get; set; }
    public virtual DbSet<AccountLikedTags> AccountsLikedTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Account

        modelBuilder.Entity<Account>()
            .HasQueryFilter(a => !a.IsDeleted);

        modelBuilder.Entity<Account>()
            .HasIndex(a => a.Nickname);

        modelBuilder.Entity<Account>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Account>()
            .Property(a => a.Nickname)
            .HasMaxLength(25)
            .IsRequired();

        modelBuilder.Entity<Account>()
            .Property(a => a.Email)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Account>()
            .Property(a => a.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder.Entity<Account>()
            .Property(a => a.Verified)
            .HasDefaultValue(false)
            .IsRequired();

        modelBuilder.Entity<Account>()
            .Property(a => a.ProfilePicUrl)
            .HasMaxLength(255);

        modelBuilder.Entity<Account>()
            .Property(a => a.BackgroundPicUrl)
            .HasMaxLength(255);

        modelBuilder.Entity<Account>()
            .Property(a => a.AccountDescription)
            .HasMaxLength(500);

        modelBuilder.Entity<Account>()
            .Property(a => a.AccountCreated)
            .HasDefaultValue(DateTime.Now);

        modelBuilder.Entity<Account>()
            .Property(a => a.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        modelBuilder.Entity<Account>()
            .Property(a => a.RoleId)
            .HasDefaultValue(1);

        #endregion

        #region Comment

        modelBuilder.Entity<Comment>()
            .HasQueryFilter(c => !c.IsDeleted);

        modelBuilder.Entity<Comment>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Comment>()
            .Property(c => c.Text)
            .HasMaxLength(500)
            .IsRequired();

        modelBuilder.Entity<Comment>()
            .Property(c => c.CommentAdded)
            .HasDefaultValue(DateTime.Now)
            .IsRequired();

        modelBuilder.Entity<Comment>()
            .Property(c => c.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        #endregion

        #region Like

        modelBuilder.Entity<Like>()
            .HasQueryFilter(l => !l.Account.IsDeleted);

        modelBuilder.Entity<Like>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<Like>()
            .Property(l => l.IsLike)
            .IsRequired();

        #endregion

        #region Picture

        modelBuilder.Entity<Picture>()
            .HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.Entity<Picture>()
            .HasIndex(p => p.Name);

        modelBuilder.Entity<Picture>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<Picture>()
            .Property(p => p.Name)
            .HasMaxLength(40)
            .IsRequired();

        modelBuilder.Entity<Picture>()
            .Property(p => p.Description)
            .HasMaxLength(500);

        modelBuilder.Entity<Picture>()
            .Property(p => p.Url)
            .HasMaxLength(250)
            .IsRequired();

        modelBuilder.Entity<Picture>()
            .Property(p => p.PictureAdded)
            .HasDefaultValue(DateTime.Now)
            .IsRequired();

        modelBuilder.Entity<Picture>()
            .Property(p => p.IsDeleted)
            .HasDefaultValue(false)
            .IsRequired();

        modelBuilder.Entity<Picture>()
            .Property(p => p.PopularityScore)
            .HasDefaultValue(36500)
            .IsRequired();

        #endregion

        #region Role

        modelBuilder.Entity<Role>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasMaxLength(16)
            .IsRequired();

        #endregion

        #region Tag

        modelBuilder.Entity<Tag>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Tag>()
            .Property(t => t.Value)
            .HasMaxLength(25);

        #endregion

        #region RestrictedIp

        modelBuilder.Entity<RestrictedIp>()
            .HasKey(i => i.Id);

        modelBuilder.Entity<RestrictedIp>()
            .Property(i => i.IpAddress)
            .HasMaxLength(256)
            .IsRequired();

        modelBuilder.Entity<RestrictedIp>()
            .Property(i => i.CantGet)
            .IsRequired();

        modelBuilder.Entity<RestrictedIp>()
            .Property(i => i.CantPost)
            .IsRequired();

        #endregion


        #region Many to many

        #region PictureTag

        modelBuilder.Entity<PictureTag>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PictureTag>()
            .HasQueryFilter(p => !p.Picture.IsDeleted);

        modelBuilder.Entity<PictureTag>()
            .HasOne(p => p.Picture)
            .WithMany(t => t.PictureTags);

        modelBuilder.Entity<PictureTag>()
            .HasOne(p => p.Tag)
            .WithMany(t => t.PictureTags);

        #endregion

        #region PicturesSeenByAccount

        modelBuilder.Entity<PictureSeenByAccount>()
            .HasQueryFilter(p => !p.Account.IsDeleted);

        modelBuilder.Entity<PictureSeenByAccount>()
            .HasKey(p => p.Id);

        modelBuilder.Entity<PictureSeenByAccount>()
            .HasOne(p => p.Picture)
            .WithMany(a => a.SeenByAccount);

        modelBuilder.Entity<PictureSeenByAccount>()
            .HasOne(p => p.Account)
            .WithMany(a => a.PicturesSeen);

        #endregion

        #region AccountLikedTagsJoin

        modelBuilder.Entity<AccountLikedTags>()
            .HasQueryFilter(p => !p.Account.IsDeleted);

        modelBuilder.Entity<AccountLikedTags>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<AccountLikedTags>()
            .HasOne(a => a.Account)
            .WithMany(atj => atj.LikedTags);

        modelBuilder.Entity<AccountLikedTags>()
            .HasOne(a => a.Tag)
            .WithMany(atj => atj.AccountLikedTags);

        #endregion

        #endregion
    }
}