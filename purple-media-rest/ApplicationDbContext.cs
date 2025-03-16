using Microsoft.EntityFrameworkCore;
using purple_media_rest.Models;

namespace purple_media_rest;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    // Add the User model to the database context
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ensure Email is unique
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Post>()
            .HasOne(p => p.Author)
            .WithMany(u => u.Posts)
            .HasForeignKey(p => p.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Post to Post (Self-Referential One-to-Many: Post has one ParentPost)
        modelBuilder.Entity<Post>()
            .HasOne(p => p.ParentPost)
            .WithMany(p => p.ChildPosts)
            .HasForeignKey(p => p.ParentPostId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid cycles

        // User to Post (Many-to-Many: LikedBy/LikedPosts)
        modelBuilder.Entity<Post>()
            .HasMany(p => p.LikedBy)
            .WithMany(u => u.LikedPosts)
            .UsingEntity(j => j.ToTable("PostLikes"));

        // Optional: Set default value for CreationDate
        modelBuilder.Entity<Post>()
            .Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

        base.OnModelCreating(modelBuilder);
    }
}