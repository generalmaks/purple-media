using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;

public class Tweet
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = default!;

    [MaxLength(300)]
    public string? Content { get; set; }

    // Replies: parent tweet
    public int? ParentTweetId { get; set; }

    [ForeignKey(nameof(ParentTweetId))]
    public Tweet? ParentTweet { get; set; }

    public List<Tweet> Replies { get; set; } = new();

    // Quote tweets / reposts
    public int? RepostId { get; set; }

    [ForeignKey(nameof(RepostId))]
    public Tweet? RepostOf { get; set; }

    public List<TweetAttachment> Attachments { get; set; } = new();

    public List<TweetLike> Likes { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;
}