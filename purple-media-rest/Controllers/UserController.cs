using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using purple_media_rest.Models;
using purple_media_rest.DTO;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(ApplicationDbContext context) : ControllerBase
{
    // GET: api/Users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetUsers()
    {
        var users = await context.Users.ToListAsync();
        var usersDto = new List<GetUserDTO>(); 
        foreach (var user in users)
        {
            var userDto = new GetUserDTO{
                Username = user.Username,
                ProfilePicturePath = user.ProfilePicturePath,
                CreatedAt = user.CreatedAt
            };
            usersDto.Add(userDto);
        }
        return Ok(usersDto);
    }

    // GET: api/Users/username
    [HttpGet("{username}")]
    public async Task<ActionResult<GetUserDTO>> GetUser(string username)
    {
        var user = await context.Users.FindAsync(username);

        if (user == null)
        {
            return NotFound();
        }

        var userDto = new GetUserDTO{
            Username = user.Username,
            ProfilePicturePath = user.ProfilePicturePath,
            CreatedAt = user.CreatedAt
        };

        return userDto;
    }

    // POST: api/Users
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(UserCreateDTO userDto)
    {
        var user = new User
        {
            Username = userDto.Username,
            Password = userDto.Password,
            Email = userDto.Email
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { username = user }, user);
    }

    // PUT: api/Users/5
    [HttpPut("{username}")]
    public async Task<IActionResult> PutUser(string username, User user)
    {
        if (username != user.Username)
        {
            return BadRequest();
        }

        context.Entry(user).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Users.Any(e => e.Username == username))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{username}")]
    public async Task<IActionResult> DeleteUser(string username)
    {
        var user = await context.Users.FindAsync(username);
        if (user == null)
        {
            return NotFound();
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/Users/5/ProfilePicture
    [HttpPut("{username}/ProfilePicture")]
    public async Task<IActionResult> UpdateProfilePicture(string username, string profilePicturePath)
    {
        var user = await context.Users.FindAsync(username);
        if (user == null)
        {
            return NotFound();
        }

        user.ProfilePicturePath = profilePicturePath;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!context.Users.Any(e => e.Username == username))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }
}    
