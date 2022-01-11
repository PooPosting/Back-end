using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PicturesAPI.Entities;

public class PictureDbContext : DbContext
{
    private string _connectionString =
        "Server=(localdb)\\mssqllocaldb;Database=PictureApiDb;Trusted_Connection=True";
        
    public DbSet<Picture> Pictures { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Dislike> Dislikes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Picture>()
            .Property(p => p.PictureAdded)
            .HasDefaultValue(DateTime.Now);
            
        modelBuilder.Entity<Account>()
            .Property(p => p.AccountCreated)
            .HasDefaultValue(DateTime.Now);
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
        
}