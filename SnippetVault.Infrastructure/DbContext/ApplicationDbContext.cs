using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SnippetVault.Core.Domain.IdentityEntities;
using SnippetVault.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace SnippetVault.Infrastructure.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<BaseUser, ApplicationRole, Guid>
    {
        public virtual DbSet<Snippet> Snippets { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Star> Stars { get; set; }
        public virtual DbSet<CommentLike> CommentLikes { get; set; }
        public new virtual DbSet<ApplicationUser> Users { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent api: add seed data if needed, alter column properties if needed
            // Fluent api table relations, if navigation property is used this not needed in ef core

            modelBuilder.Entity<Comment>().ToTable("Comments");
            modelBuilder.Entity<CommentLike>().ToTable("CommentLikes");
            modelBuilder.Entity<Snippet>().ToTable("Snippets");
            modelBuilder.Entity<Star>().ToTable("Stars");
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");

            modelBuilder.Entity<Comment>()
                .HasMany(c => c.CommentLikes)
                .WithOne(cl => cl.Comment)
                .HasForeignKey(cl => cl.CommentId)
                .HasPrincipalKey(c => c.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Snippet>()
                .HasMany(s => s.Comments)
                .WithOne(c => c.Snippet)
                .HasForeignKey(c => c.CommentSnippetId)
                .HasPrincipalKey(s => s.SnippetId)
                .OnDelete(DeleteBehavior.Cascade); // when snippet is deleted delete all comments of it

            modelBuilder.Entity<Snippet>()
                .HasMany(s => s.Stars)
                .WithOne(st => st.Snippet)
                .HasForeignKey(st => st.SnippetId)
                .HasPrincipalKey(s => s.SnippetId)
                .OnDelete(DeleteBehavior.Cascade); // when snippet is deleted delete all stars of it

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.Comments)
                .WithOne(c => c.OwnerUser)
                .HasForeignKey(c => c.OwnerUserId)
                .HasPrincipalKey(a => a.Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.CommentLikes)
                .WithOne(cl => cl.OwnerUser)
                .HasForeignKey(cl => cl.OwnerUserId)
                .HasPrincipalKey(a => a.Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.Snippets)
                .WithOne(s => s.OwnerUser)
                .HasForeignKey(s => s.OwnerUserId)
                .HasPrincipalKey(a => a.Id)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(a => a.Stars)
                .WithOne(st => st.OwnerUser)
                .HasForeignKey(st => st.OwnerUserId)
                .HasPrincipalKey(a => a.Id)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}