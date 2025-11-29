using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public Task<User?> GetByIdAsync(int id) =>
        _db.Users.FirstOrDefaultAsync(u => u.Id == id);

    public Task<User?> GetByUserNameAsync(string username) =>
        _db.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<User> CreateAsync(User user)
    {
        if (await _db.Users.FirstOrDefaultAsync(u => u.Username == user.Username) is not null)
            throw new Exception("User with this name already exists.");
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return user;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _db.Users.Update(user);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await GetByIdAsync(id);
        if (user == null) return false;

        _db.Users.Remove(user);
        return await _db.SaveChangesAsync() > 0;
    }
}