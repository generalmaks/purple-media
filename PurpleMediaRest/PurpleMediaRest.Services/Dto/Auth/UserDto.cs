namespace PurpleMediaRest.Services.Dto.Auth;

public record UserDto(
    int Id,
    string Username,
    string DisplayName,
    string Bio,
    string ProfilePictureUrl,
    DateTime CreatedAt);