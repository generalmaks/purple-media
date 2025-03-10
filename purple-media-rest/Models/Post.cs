using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace purple_media_rest.Models;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PostId { get; set; }
    [Required]
    public string Content { get; set; } = "";
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Required]
    public int UserId { get; set; }
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }
    [Required]
    public List<Comment> Comments { get; set; } = [];
    public int CommentsCount { get; set; } = 0;
    public List<User> LikedBy { get; set; } = [];
    public int Likes { get; set; } = 0;
}