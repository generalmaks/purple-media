using System.ComponentModel.DataAnnotations;

namespace purple_media_rest.DTO;
public record UserCreateDTO
{
    [Required]
    public string Username { get; set; } = "";
    [Required]
    public string Password { get; set; } = "";
    [Required, EmailAddress]
    public string Email { get; set; } = "";
}
