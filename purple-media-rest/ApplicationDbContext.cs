using Microsoft.EntityFrameworkCore;
using purple_media_rest.Models;

namespace purple_media_rest;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<FileAttachment> FileAttachment { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Post>()
            .HasOne(p => p.ParentPost)
            .WithMany(p => p.ChildPosts)
            .HasForeignKey(p => p.ParentPostId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Post>()
            .HasMany(p => p.LikedBy)
            .WithMany(u => u.LikedPosts)
            .UsingEntity(j => j.ToTable("PostLikes"));

        modelBuilder.Entity<FileAttachment>()
            .HasOne(f => f.Owner);

        base.OnModelCreating(modelBuilder);
    }
}