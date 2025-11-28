namespace PurpleMediaRest.Services.Interfaces;

public interface ILikeService
{
    Task<bool> LikeAsync(int userId, int tweetId);
    Task<bool> UnlikeAsync(int userId, int tweetId);
    Task<bool> IsLikedAsync(int userId, int tweetId);
    Task<int> CountAsync(int tweetId);
}