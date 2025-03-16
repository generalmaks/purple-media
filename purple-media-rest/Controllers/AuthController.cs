using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using purple_media_rest;
using purple_media_rest.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PurpleMediaRest.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Authenticate user
            var token = await AuthenticateUserAsync(userCreateDTO);
            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(new { Token = token });
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDTO userCreateDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Register user
            var result = await RegisterUserAsync(userCreateDTO);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        private async Task<string> AuthenticateUserAsync(UserCreateDTO userCreateDTO)
        {
            var user = await Task.Run(() => _context.Users.SingleOrDefault(
                u => u.Username == userCreateDTO.Username && u.Password == userCreateDTO.Password));
            
            if (user == null)
            {
                return null;
            }

            return GenerateJwtToken(user);
        }

        private async Task<IdentityResult> RegisterUserAsync(UserCreateDTO userCreateDTO)
        {
            var user = new User
            {
                Username = userCreateDTO.Username,
                Password = userCreateDTO.Password // Note: In a real application, you should hash the password before storing it
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        [HttpGet("token")]
        private string GenerateJwtToken(User user) // Changed to take User instead of UserCreateDTO
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],    // "http://localhost:5173"
                audience: _config["Jwt:Audience"], // "http://localhost:5173"
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class IdentityResult
    {
        public bool Succeeded { get; set; }
        public string[]? Errors { get; set; }
        public static IdentityResult Success => new IdentityResult { Succeeded = true };
    }
}