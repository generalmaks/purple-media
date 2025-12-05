using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Auth;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class UserService(AppDbContext db) : IUserService
{
    public Task<UserDto?> GetByIdAsync(int id) =>
        db.Users.Where(u => u.Id == id)
            .Select(u => new UserDto(
                u.Id,
                u.Username,
                u.DisplayName,
                u.Bio!,
                u.ProfilePictureUrl!,
                u.CreatedAt
            ))
            .FirstOrDefaultAsync();

    public Task<User?> GetByUserNameAsync(string username) =>
        db.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User> CreateAsync(User user)
    {
        if (await db.Users.FirstOrDefaultAsync(u => u.Username == user.Username) is not null)
            throw new Exception("User with this name already exists.");
        db.Users.Add(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        db.Users.Update(user);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var userDto = await GetByIdAsync(id);
        if (userDto == null) return false;

        var user = await db.Users.FindAsync(userDto.Id);

        db.Users.Remove(user!);
        return await db.SaveChangesAsync() > 0;
    }
}