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
                u => u.Username == userCreateDTO.Username && u.PasswordHash == userCreateDTO.Password));
            
            if (user == null)
            {
                return null;
            }

            return GenerateJwtToken(userCreateDTO);
        }

        private async Task<IdentityResult> RegisterUserAsync(UserCreateDTO userCreateDTO)
        {
            var user = new User
            {
                Username = userCreateDTO.Username,
                PasswordHash = userCreateDTO.Password // Note: In a real application, you should hash the password before storing it
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        [HttpGet("token")]
        public string GenerateJwtToken(UserCreateDTO userCreateDTO)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userCreateDTO.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

    public class IdentityResult
    {
        public bool Succeeded { get; set; }
        public string[]? Errors { get; set; }
        public static IdentityResult Success => new IdentityResult { Succeeded = true };
    }
}