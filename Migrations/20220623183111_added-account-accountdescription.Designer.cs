﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PicturesAPI.Entities;

#nullable disable

namespace PicturesAPI.Migrations
{
    [DbContext(typeof(PictureDbContext))]
    [Migration("20220623183111_added-account-accountdescription")]
    partial class addedaccountaccountdescription
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PicturesAPI.Entities.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("AccountCreated")
                        .HasColumnType("datetime");

                    b.Property<string>("AccountDescription")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("BackgroundPicUrl")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<string>("ProfilePicUrl")
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<bool>("Verified")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CommentAdded")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("PictureId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("PictureId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Joins.AccountLikedTagsJoin", b =>
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

                    b.ToTable("AccountLikedTagJoins");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Joins.PictureSeenByAccountJoin", b =>
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

                    b.ToTable("PictureAccountJoins");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Joins.PictureTagJoin", b =>
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

                    b.ToTable("PictureTagJoins");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("IsLike")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("LikedId")
                        .HasColumnType("int");

                    b.Property<int?>("LikerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LikedId");

                    b.HasIndex("LikerId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Picture", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("varchar(500)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("varchar(40)");

                    b.Property<DateTime>("PictureAdded")
                        .HasColumnType("datetime");

                    b.Property<long>("PopularityScore")
                        .HasColumnType("bigint");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("PicturesAPI.Entities.RestrictedIp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("CantGet")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("CantPost")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(128)
                        .HasColumnType("varchar(128)");

                    b.HasKey("Id");

                    b.ToTable("RestrictedIps");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(16)
                        .HasColumnType("varchar(16)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Value")
                        .HasMaxLength(25)
                        .HasColumnType("varchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Account", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Comment", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Account", "Account")
                        .WithMany("Comments")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PicturesAPI.Entities.Picture", "Picture")
                        .WithMany("Comments")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Joins.AccountLikedTagsJoin", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Account", "Account")
                        .WithMany("AccountLikedTagJoins")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PicturesAPI.Entities.Tag", "Tag")
                        .WithMany("AccountLikedTagJoins")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Joins.PictureSeenByAccountJoin", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Picture", "Picture")
                        .WithMany("PictureAccountJoins")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PicturesAPI.Entities.Account", "Account")
                        .WithMany("PictureAccountJoins")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Picture");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Joins.PictureTagJoin", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Picture", "Picture")
                        .WithMany("PictureTagJoins")
                        .HasForeignKey("PictureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PicturesAPI.Entities.Tag", "Tag")
                        .WithMany("PictureTagJoins")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Picture");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Like", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Picture", "Liked")
                        .WithMany("Likes")
                        .HasForeignKey("LikedId");

                    b.HasOne("PicturesAPI.Entities.Account", "Liker")
                        .WithMany("Likes")
                        .HasForeignKey("LikerId");

                    b.Navigation("Liked");

                    b.Navigation("Liker");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Picture", b =>
                {
                    b.HasOne("PicturesAPI.Entities.Account", "Account")
                        .WithMany("Pictures")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Account", b =>
                {
                    b.Navigation("AccountLikedTagJoins");

                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("PictureAccountJoins");

                    b.Navigation("Pictures");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Picture", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Likes");

                    b.Navigation("PictureAccountJoins");

                    b.Navigation("PictureTagJoins");
                });

            modelBuilder.Entity("PicturesAPI.Entities.Tag", b =>
                {
                    b.Navigation("AccountLikedTagJoins");

                    b.Navigation("PictureTagJoins");
                });
#pragma warning restore 612, 618
        }
    }
}
