using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PurpleMediaRest.DataAccess.Enums;
using PurpleMediaRest.Services.Dto.Auth;
using PurpleMediaRest.Services.Interfaces;
using TwitterClone.Data;

namespace PurpleMediaRest.Services.Services;

public class AuthService(
    AppDbContext db,
    IUserService userService,
    IConfiguration config) : IAuthService
{
    public async Task RegisterAsync(RegisterDto registerDto)
    {
        if (await db.Users.AnyAsync(u => u.Username == registerDto.Username))
            throw new Exception("User with this username already exists");
        var user = new User
        {
            Id = 0,
            Username = registerDto.Username,
            DisplayName = registerDto.DisplayName,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.UnhashedPassword),
            Bio = null,
            ProfilePictureUrl = null,
            UserRole = UserRole.User,
            CreatedAt = DateTime.UtcNow,
            Tweets = null,
            Likes = null,
            Following = null,
            Followers = null
        };

        await userService.CreateAsync(user);
    }

    public async Task<string> LoginAsync(LoginDto loginDto)
    {
        var foundUser = await db.Users.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
        if (foundUser is null || !BCrypt.Net.BCrypt.Verify(loginDto.UnhashedPassword, foundUser.HashedPassword))
            throw new KeyNotFoundException("Invalid credentials");
        return GenerateToken(foundUser.Id, foundUser.Username, foundUser.UserRole.ToString());
    }

    private string GenerateToken(int userId, string username, string userRole)
    {
        var key = Encoding.ASCII.GetBytes(config["Jwt:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, userRole)
            }),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt64(config["Jwt:ExpireTimeInMinutes"])),
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}