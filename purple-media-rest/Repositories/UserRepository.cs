using System.Collections;
using Microsoft.EntityFrameworkCore;
using purple_media_rest.DTO;
using purple_media_rest.Models;

namespace purple_media_rest.Repositories;

public class UserRepository(ApplicationDbContext context)
{
    public async Task<IEnumerable> GetAllUsers()
    {
        var users = await context.Users.ToListAsync();
        return users.Select(user => new GetUserDTO
        {
            Username = user.Username, 
            ProfilePictureId = user.ProfilePictureId,
            CreatedAt = user.CreatedAt
        }).ToList();
    }

    public async Task<GetUserDTO> GetUser(string username)
    {
        var user = await context.Users.FindAsync(username);

        if (user == null)
            throw new Exception($"User {username} not found.");
        var userDto = new GetUserDTO{
            Username = user.Username,
            ProfilePictureId = user.ProfilePictureId,
            CreatedAt = user.CreatedAt
        };

        return userDto;
    }

    public async Task<User> PostUser(UserCreateDTO userDto)
    {
        var user = new User
        {
            Username = userDto.Username,
            Password = userDto.Password,
            Email = userDto.Email
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUser(string username)
    {
        var user = await context.Users.FindAsync(username);
        if (user == null)
            throw new Exception($"User {username} not found.");
        
        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
    
    public async Task UpdateProfilePicture(string username, int profilePictureId)
    {
        var user = await context.Users.FindAsync(username);
        if (user == null)
            throw new Exception($"User {username} not found.");

        user.ProfilePictureId = profilePictureId;
        await context.SaveChangesAsync();
    }
}