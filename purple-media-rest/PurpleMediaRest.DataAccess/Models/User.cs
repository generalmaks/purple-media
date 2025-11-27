using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.DataAccess.Enums;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(32)]
    public string Username { get; set; } = null!;

    [Required, MaxLength(64)]
    public string DisplayName { get; set; } = null!;

    [MaxLength(256)]
    public string? Bio { get; set; }

    [MaxLength(256)]
    public string? ProfilePictureUrl { get; set; }

    public UserRole UserRole { get; set; } = UserRole.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public List<Tweet> Tweets { get; set; } = [];
    public List<TweetLike> Likes { get; set; } = [];

    public List<Follow> Following { get; set; } = [];
    public List<Follow> Followers { get; set; } = [];
}