using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using purple_media_rest.DTO;
using purple_media_rest.Models;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration config) : Controller
{
  private static readonly List<User> Users = [];

  [HttpPost("register")]
  public IActionResult Register(RegisterDTO request)
  {
    if(Users.Any(u => u.Username == request.Username))
      return BadRequest("Username already exists");
    
    Users.Add(new User { Username = request.Username, Password = request.Password });
    return Ok("User created");
  }

  [HttpPost("login")]
  public IActionResult Login(LoginDTO request)
  {
    var user = Users.SingleOrDefault(u => u.Username == request.Username);
    if (user == null || user.Password != request.Password)
      return Unauthorized("Invalid credentials");
    
    var token = GenerateJwtToken(user);
    
    return Ok(new { token });
  }
  
  private string GenerateJwtToken(User user)
  {
    var claims = new[]
    {
      new Claim(JwtRegisteredClaimNames.Sub, user.Username),
      new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var token = new JwtSecurityToken(
      issuer: config["Jwt:Issuer"],
      audience: config["Jwt:Audience"],
      claims: claims,
      expires: DateTime.UtcNow.AddHours(1),
      signingCredentials: creds
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
  }
}