using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;

namespace TwitterClone.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Tweet> Tweets => Set<Tweet>();
    public DbSet<TweetAttachment> Attachments => Set<TweetAttachment>();
    public DbSet<TweetLike> Likes => Set<TweetLike>();
    public DbSet<Follow> Follows => Set<Follow>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // -------------------------
        // User
        // -------------------------
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        // -------------------------
        // Tweet (includes comments & retweets)
        // -------------------------
        modelBuilder.Entity<Tweet>(entity =>
        {
            entity.HasOne(t => t.Author)
                .WithMany(u => u.Tweets)
                .HasForeignKey(t => t.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.ParentTweet)
                .WithMany(t => t.Replies)
                .HasForeignKey(t => t.ParentTweetId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(t => t.Text)
                  .HasMaxLength(280);

            entity.Property(t => t.CreatedAt)
                  .HasDefaultValueSql("NOW()");
        });

        // -------------------------
        // Attachments
        // -------------------------
        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasOne(a => a.Tweet)
                .WithMany(t => t.Attachments)
                .HasForeignKey(a => a.TweetId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(a => a.Url)
                  .HasMaxLength(500);
        });

        // -------------------------
        // Likes (many-to-many but explicit table)
        // -------------------------
        modelBuilder.Entity<TweetLike>(entity =>
        {
            entity.HasKey(l => new { l.UserId, l.TweetId });

            entity.HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(l => l.Tweet)
                .WithMany(t => t.Likes)
                .HasForeignKey(l => l.TweetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // -------------------------
        // Follows (self-referencing many-to-many)
        // -------------------------
        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(f => new { f.FollowerId, f.FollowingId });

            entity.HasOne(f => f.Follower)
                .WithMany(u => u.Following)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(f => f.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
