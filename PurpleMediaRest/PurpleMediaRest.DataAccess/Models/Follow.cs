using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PurpleMediaRest.DataAccess.Models;

public class Follow
{
    [Key]
    public int Id { get; set; }

    // Who follows
    [Required]
    public int FollowerId { get; set; }

    [ForeignKey(nameof(FollowerId))]
    public User Follower { get; set; } = default!;

    // Who is being followed
    [Required]
    public int FollowedId { get; set; }

    [ForeignKey(nameof(FollowedId))]
    public User Followed { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}