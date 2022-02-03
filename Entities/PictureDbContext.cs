using Microsoft.EntityFrameworkCore;

namespace PicturesAPI.Entities;

public class PictureDbContext : DbContext
{
    public PictureDbContext(DbContextOptions<PictureDbContext> options) : base(options) { }
        
    public virtual DbSet<Picture> Pictures { get; set; }
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<Like> Likes { get; set; }
    
    // protected override void OnModelCreating(ModelBuilder modelBuilder) { }
}