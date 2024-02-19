﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SnippetVault.Infrastructure.DbContext;

#nullable disable

namespace SnippetVault.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.Comment", b =>
                {
                    b.Property<Guid?>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CommentBody")
                        .IsRequired()
                        .HasMaxLength(8192)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CommentCreatedDateTime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("CommentLastUpdatedDateTime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("CommentSnippetId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Hidden")
                        .HasColumnType("bit");

                    b.Property<Guid?>("OwnerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CommentId");

                    b.HasIndex("CommentSnippetId");

                    b.HasIndex("OwnerUserId");

                    b.ToTable("Comments", (string)null);
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.CommentLike", b =>
                {
                    b.Property<Guid?>("CommentLikeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CommentId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<short>("CommentLikeSize")
                        .HasColumnType("smallint");

                    b.Property<Guid?>("OwnerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("CommentLikeId");

                    b.HasIndex("CommentId");

                    b.HasIndex("OwnerUserId");

                    b.ToTable("CommentLikes", (string)null);
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.Snippet", b =>
                {
                    b.Property<Guid?>("SnippetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Hidden")
                        .HasColumnType("bit");

                    b.Property<Guid?>("OwnerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SnippetBody")
                        .IsRequired()
                        .HasMaxLength(32768)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SnippetCreatedDateTime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("SnippetDescription")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<string>("SnippetFileName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime?>("SnippetLastUpdateDateTime")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("SnippetTitle")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("nvarchar(512)");

                    b.HasKey("SnippetId");

                    b.HasIndex("OwnerUserId");

                    b.ToTable("Snippets", (string)null);
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.Star", b =>
                {
                    b.Property<Guid?>("StarId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("LastUpdateTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("OwnerUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("SnippetId")
                        .IsRequired()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("StarActive")
                        .HasColumnType("bit");

                    b.HasKey("StarId");

                    b.HasIndex("OwnerUserId");

                    b.HasIndex("SnippetId");

                    b.ToTable("Stars", (string)null);
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.IdentityEntities.ApplicationRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.IdentityEntities.BaseUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.IdentityEntities.ApplicationUser", b =>
                {
                    b.HasBaseType("SnippetVault.Core.Domain.IdentityEntities.BaseUser");

                    b.Property<string>("AvatarImage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastSentConfirmEmailTime")
                        .HasColumnType("datetime2");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.BaseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.BaseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.BaseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.BaseUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.Comment", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.Entities.Snippet", "Snippet")
                        .WithMany("Comments")
                        .HasForeignKey("CommentSnippetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.ApplicationUser", "OwnerUser")
                        .WithMany("Comments")
                        .HasForeignKey("OwnerUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("OwnerUser");

                    b.Navigation("Snippet");
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.CommentLike", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.Entities.Comment", "Comment")
                        .WithMany("CommentLikes")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.ApplicationUser", "OwnerUser")
                        .WithMany("CommentLikes")
                        .HasForeignKey("OwnerUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Comment");

                    b.Navigation("OwnerUser");
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.Snippet", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.ApplicationUser", "OwnerUser")
                        .WithMany("Snippets")
                        .HasForeignKey("OwnerUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("OwnerUser");
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.Star", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.ApplicationUser", "OwnerUser")
                        .WithMany("Stars")
                        .HasForeignKey("OwnerUserId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("SnippetVault.Core.Domain.Entities.Snippet", "Snippet")
                        .WithMany("Stars")
                        .HasForeignKey("SnippetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OwnerUser");

                    b.Navigation("Snippet");
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.IdentityEntities.ApplicationUser", b =>
                {
                    b.HasOne("SnippetVault.Core.Domain.IdentityEntities.BaseUser", null)
                        .WithOne()
                        .HasForeignKey("SnippetVault.Core.Domain.IdentityEntities.ApplicationUser", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.Comment", b =>
                {
                    b.Navigation("CommentLikes");
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.Entities.Snippet", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Stars");
                });

            modelBuilder.Entity("SnippetVault.Core.Domain.IdentityEntities.ApplicationUser", b =>
                {
                    b.Navigation("CommentLikes");

                    b.Navigation("Comments");

                    b.Navigation("Snippets");

                    b.Navigation("Stars");
                });
#pragma warning restore 612, 618
        }
    }
}
