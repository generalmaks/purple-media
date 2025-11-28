namespace PurpleMediaRest.Services.Dto.Auth;

public record RegisterDto(string username, string displayName, string unhashedPassword);