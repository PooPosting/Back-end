﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PooPosting.Domain.DbContext;

#nullable disable

namespace PooPosting.Domain.Migrations
{
    [DbContext(typeof(PictureDbContext))]
    partial class PictureDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AccountCreated")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProfilePicUrl")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(64)
                        .HasColumnType("varchar(64)");

                    b.Property<DateTime?>("RefreshTokenExpires")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<bool>("Verified")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("Nickname");

                    b.HasIndex("RoleId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CommentAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("PictureId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("CommentAdded");

                    b.HasIndex("PictureId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Joins.AccountLikedTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("TagId");

                    b.ToTable("AccountsLikedTags");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Joins.PictureSeenByAccount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<int>("PictureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("PictureId");

                    b.ToTable("PicturesSeenByAccounts");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Joins.PictureTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("PictureId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PictureId");

                    b.HasIndex("TagId");

                    b.ToTable("PictureTags");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Liked")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("PictureId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("Liked");

                    b.HasIndex("PictureId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("PictureAdded")
                        .HasColumnType("datetime(6)");

                    b.Property<long>("PopularityScore")
                        .HasColumnType("bigint");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("PictureAdded");

                    b.HasIndex("PopularityScore");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("Id");

                    b.HasIndex("Value");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Account", b =>
                {
                    b.HasOne("PooPosting.Domain.DbContext.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Comment", b =>
                {
                    b.HasOne("PooPosting.Domain.DbContext.Entities.Account", "Account")
                        .WithMany("Comments")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PooPosting.Domain.DbContext.Entities.Picture", "Picture")
                        .WithMany("Comments")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Joins.AccountLikedTag", b =>
                {
                    b.HasOne("PooPosting.Domain.DbContext.Entities.Account", "Account")
                        .WithMany("LikedTags")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PooPosting.Domain.DbContext.Entities.Tag", "Tag")
                        .WithMany("AccountLikedTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Joins.PictureSeenByAccount", b =>
                {
                    b.HasOne("PooPosting.Domain.DbContext.Entities.Account", "Account")
                        .WithMany("PicturesSeen")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PooPosting.Domain.DbContext.Entities.Picture", "Picture")
                        .WithMany("SeenByAccount")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Joins.PictureTag", b =>
                {
                    b.HasOne("PooPosting.Domain.DbContext.Entities.Picture", "Picture")
                        .WithMany("PictureTags")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PooPosting.Domain.DbContext.Entities.Tag", "Tag")
                        .WithMany("PictureTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Picture");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Like", b =>
                {
                    b.HasOne("PooPosting.Domain.DbContext.Entities.Account", "Account")
                        .WithMany("Likes")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PooPosting.Domain.DbContext.Entities.Picture", "Picture")
                        .WithMany("Likes")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Picture", b =>
                {
                    b.HasOne("PooPosting.Domain.DbContext.Entities.Account", "Account")
                        .WithMany("Pictures")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Account", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("LikedTags");

                    b.Navigation("Likes");

                    b.Navigation("Pictures");

                    b.Navigation("PicturesSeen");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Picture", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("PictureTags");

                    b.Navigation("SeenByAccount");
                });

            modelBuilder.Entity("PooPosting.Domain.DbContext.Entities.Tag", b =>
                {
                    b.Navigation("AccountLikedTags");

                    b.Navigation("PictureTags");
                });
#pragma warning restore 612, 618
        }
    }
}
