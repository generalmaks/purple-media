using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using purple_media_rest.DTO;

namespace purple_media_rest.Models;

public class User
{
    [Key]
    [Column(TypeName = "varchar(50)")]
    public string Username { get; init; } = "";
    [Required]
    [Column(TypeName = "varchar(100)")]
    [EmailAddress]
    public string Email { get; init; } = "";
    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Password { get; init; } = "";
    [Column(TypeName = "varchar(255)")]
    public string? ProfilePicturePath { get; set; } = string.Empty;
    [Column(TypeName = "tinyint")]
    public byte IsAdmin { get; init; } = 0;
    
    [Required]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public List<Post> Posts { get; init; } = [];
    public List<Post> LikedPosts { get; init; } = [];

    public GetUserDTO GetUserDto(){
        var userDto = new GetUserDTO{
            Username = this.Username,
            ProfilePicturePath = this.ProfilePicturePath,
            CreatedAt = this.CreatedAt
        };
        return userDto;
    }
}