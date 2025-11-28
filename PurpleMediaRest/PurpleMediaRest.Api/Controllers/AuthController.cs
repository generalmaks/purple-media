using Microsoft.AspNetCore.Mvc;
using PurpleMediaRest.Services.Dto.Auth;
using PurpleMediaRest.Services.Interfaces;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/aith")]
public class AuthController(IAuthService service) : ControllerBase
{
    [HttpPost("register/{username}/{displayName}/{unhashedPassword}")]
    public async Task<ActionResult> Register(string username, string displayName, string unhashedPassword)
    {
        try
        {
            await service.RegisterAsync(new RegisterDto(username, displayName, unhashedPassword));
            return Created();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login/{username}/{unhashedPassword}")]
    public async Task<ActionResult<string>> Login(string username, string unhashedPassword)
    {
        try
        {
            var token = service.LoginAsync(new LoginDto(username, unhashedPassword));
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}