using PurpleMediaRest.DataAccess.Models;

namespace PurpleMediaRest.Services.Interfaces;

public interface ITweetService
{
    Task<Tweet?> GetAsync(int id);
    Task<IEnumerable<Tweet>> GetLatestAsync(int page, int pageSize);
    Task<IEnumerable<Tweet>> GetUserTweetsAsync(int userId);
    Task<Tweet> CreateAsync(int authorId, string text, int? parentTweetId = null);
    Task<bool> DeleteAsync(int id);
}