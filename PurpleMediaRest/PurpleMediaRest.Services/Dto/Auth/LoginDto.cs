namespace PurpleMediaRest.Services.Dto.Auth;

public record LoginDto(string Username, string UnhashedPassword);