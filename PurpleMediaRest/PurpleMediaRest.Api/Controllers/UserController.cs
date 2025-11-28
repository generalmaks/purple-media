using Microsoft.AspNetCore.Mvc;
using PurpleMediaRest.DataAccess.Enums;
using PurpleMediaRest.Services.Interfaces;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<User?>> GetAsync(int userId)
    {
        try
        {
            var user = await service.GetByIdAsync(userId);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("by-username/{username}")]
    public async Task<ActionResult<User?>> GetByUsernameAsync(string username)
    {
        try
        {
            var user = await service.GetByUserNameAsync(username);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{username}/{displayName}")]
    public async Task<ActionResult<User>> CreateAsync(string username, string displayName)
    {
        try
        {
            var user = new User
            {
                Username = username,
                DisplayName = displayName,
                Bio = null,
                ProfilePictureUrl = null,
                UserRole = UserRole.User,
                CreatedAt = DateTime.UtcNow,
            };

            var createdUser = await service.CreateAsync(user);
            return Created();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{userId:int}")]
    public async Task<ActionResult> UpdateAsync(int userId, [FromBody] User updatedUser)
    {
        try
        {
            if (updatedUser.Id != userId)
                return BadRequest("User ID mismatch");

            var result = await service.UpdateAsync(updatedUser);
            if (!result) return NotFound();

            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{userId:int}")]
    public async Task<ActionResult<bool>> DeleteAsync(int userId)
    {
        try
        {
            var isDeleted = await service.DeleteAsync(userId);
            return Ok(isDeleted);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
