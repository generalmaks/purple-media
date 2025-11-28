using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace purple_media_rest.PurpleMediaRest.DataAccess.Models;

public class TweetLike
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TweetId { get; set; }

    [ForeignKey(nameof(TweetId))]
    public Tweet Tweet { get; set; } = default!;

    [Required]
    public int UserId { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = default!;
}