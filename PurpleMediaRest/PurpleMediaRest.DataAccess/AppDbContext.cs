using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess.Models;

namespace PurpleMediaRest.DataAccess;

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
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(u => u.Following)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Followed)
            .WithMany(u => u.Followers)
            .HasForeignKey(f => f.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ChatMessage>()
            .HasOne(u => u.Receiver)
            .WithMany(m => m.MessagesReceived)
            .HasForeignKey(u => u.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ChatMessage>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessagesSent)
            .HasForeignKey(u => u.SenderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
