using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace purple_media_rest.Models;

public class User
{
    [Key]
    [Column(TypeName = "varchar(50)")]
    public string Username { get; set; } = "";
    [Required]
    [Column(TypeName = "varchar(100)")]
    [EmailAddress]
    public string Email { get; set; } = "";
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Password { get; set; } = "";
    [Column(TypeName = "varchar(255)")]
    public string? ProfilePicturePath { get; set; } = string.Empty;
    [Column(TypeName = "tinyint")]
    public byte IsAdmin { get; set; } = 0;
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Post> Posts { get; set; } = [];
    public List<Post> LikedPosts { get; set; } = [];
}