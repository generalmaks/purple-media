using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Auth;

namespace PurpleMediaRest.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(int id);
    Task<User?> GetByUserNameAsync(string username);
    Task<User> CreateAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}