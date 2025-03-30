namespace purple_media_rest.DTO;
public record UserCreateDTO
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
}
