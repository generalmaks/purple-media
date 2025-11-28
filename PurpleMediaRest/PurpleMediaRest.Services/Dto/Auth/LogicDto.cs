namespace PurpleMediaRest.Services.Dto.Auth;

public record LoginDto(string username, string unhashedPassword);