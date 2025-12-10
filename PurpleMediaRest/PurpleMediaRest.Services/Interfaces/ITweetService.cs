using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Tweet;

namespace PurpleMediaRest.Services.Interfaces;

public interface ITweetService
{
    Task<TweetDto?> GetAsync(int id);
    Task<IEnumerable<TweetDto>> GetLatestAsync(int page, int pageSize);
    Task<IEnumerable<TweetDto>> GetUserTweetsAsync(int userId);
    Task<IEnumerable<TweetDto>> GetResponsesToTweetAsync(int tweetId);
    Task<TweetDto> CreateAsync(int authorId, string text, int? parentTweetId = null);
    Task<bool> DeleteAsync(int id);
}