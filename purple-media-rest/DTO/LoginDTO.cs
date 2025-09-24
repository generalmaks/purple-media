namespace purple_media_rest.DTO;

public class LoginDTO
{
  public required string Username { get; init; } = string.Empty;
  public required string Password { get; init; } = string.Empty;
}