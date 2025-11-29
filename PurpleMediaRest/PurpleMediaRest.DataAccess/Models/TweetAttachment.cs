using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PurpleMediaRest.DataAccess.Models;

public class TweetAttachment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int TweetId { get; set; }

    [ForeignKey(nameof(TweetId))]
    [JsonIgnore]
    public Tweet Tweet { get; set; } = null!;

    [Required, JsonIgnore] public byte[] Data { get; set; } = null!;

    [Required, MaxLength(32)]
    public string MediaType { get; set; } = null!;

    [Required, MaxLength(256)] public string FileName { get; set; } = null!;
}