using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using purple_media_rest.Models;
using purple_media_rest.DTO;
using purple_media_rest.Repositories;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(UserRepository userRepository) : ControllerBase
{
    // GET: api/Users
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetUsers()
    {
        try
        {
            var users = await userRepository.GetAllUsers();
            return Ok(users);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // GET: api/Users/username
    [HttpGet("{username}")]
    public async Task<ActionResult<GetUserDTO>> GetUser(string username)
    {
        try
        {
            var user = await userRepository.GetUser(username);
            return Ok(user);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(UserCreateDTO userDto)
    {
        try
        {
            await userRepository.PostUser(userDto);
            return Ok("User created");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        try
        {
            await userRepository.DeleteUser(username);
            return Ok("User deleted");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // PUT: api/Users/5/ProfilePicture
    [HttpPut("{username}/ProfilePicture")]
    public async Task<IActionResult> UpdateProfilePicture(string username, int profilePictureId)
    {
        try
        {
            await userRepository.UpdateProfilePicture(username, profilePictureId);
            return Ok("Profile picture updated");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}    
