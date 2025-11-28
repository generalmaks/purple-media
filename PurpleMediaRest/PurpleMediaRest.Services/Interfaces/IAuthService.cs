using PurpleMediaRest.Services.Dto.Auth;

namespace PurpleMediaRest.Services.Interfaces;

public interface IAuthService
{
    Task RegisterAsync(RegisterDto registerDto);
    Task<string> LoginAsync(LoginDto loginDto);
}