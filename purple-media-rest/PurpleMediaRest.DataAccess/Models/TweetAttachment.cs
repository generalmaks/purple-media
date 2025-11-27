using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace purple_media_rest.PurpleMediaRest.DataAccess.Models;

public class TweetAttachment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TweetId { get; set; }

    [ForeignKey(nameof(TweetId))]
    public Tweet Tweet { get; set; } = default!;

    [Required, MaxLength(256)]
    public string Url { get; set; } = default!;

    [Required, MaxLength(32)]
    public string MediaType { get; set; } = default!;
}