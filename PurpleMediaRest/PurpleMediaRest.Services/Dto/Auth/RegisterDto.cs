namespace PurpleMediaRest.Services.Dto.Auth;

public record RegisterDto(string Username, string DisplayName, string UnhashedPassword);