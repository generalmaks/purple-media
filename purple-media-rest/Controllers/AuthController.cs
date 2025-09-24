using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using purple_media_rest.DTO;
using purple_media_rest.Models;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration config, ApplicationDbContext context) : Controller
{

  [HttpPost("register")]
  public async Task<IActionResult> Register(RegisterDTO request)
  {
    var users = await context.Users.ToListAsync();
    if(users.Any(u => u.Username == request.Username))
      return BadRequest("Username already exists");
    
    context.Users.Add(new User { Username = request.Username, Password = request.Password, Email = request.Email });
    await context.SaveChangesAsync();
    return Ok(new {message = "User created"});
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login(LoginDTO request)
  {
    var user = await context.Users.SingleOrDefaultAsync(u => u.Username == request.Username);
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