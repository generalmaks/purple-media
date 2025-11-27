using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;

public class Tweet
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int AuthorId { get; set; }

    [ForeignKey(nameof(AuthorId))]
    public User Author { get; set; } = default!;

    [MaxLength(300)]
    public string? Content { get; set; }

    // Replies: parent tweet
    public int? ParentTweetId { get; set; }

    [ForeignKey(nameof(ParentTweetId))]
    public Tweet? ParentTweet { get; set; }

    public List<Tweet> Replies { get; set; } = new();

    public List<TweetAttachment> Attachments { get; set; } = new();

    public List<TweetLike> Likes { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; } = false;
}