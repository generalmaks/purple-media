namespace PurpleMediaRest.Services.Interfaces;

public interface IFollowService
{
    Task<bool> FollowAsync(int followerId, int followingId);
    Task<bool> UnfollowAsync(int followerId, int followingId);
    Task<bool> IsFollowingAsync(int followerId, int followingId);
    Task<int> FollowersCountAsync(int userId);
    Task<int> FollowingCountAsync(int userId);
}