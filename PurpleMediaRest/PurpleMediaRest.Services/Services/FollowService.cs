using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class FollowService(AppDbContext db) : IFollowService
{
    public async Task<bool> FollowAsync(int followerId, int followingId)
    {
        if (followerId == followingId) return false;
        if (await IsFollowingAsync(followerId, followingId)) return false;

        db.Follows.Add(new Follow { FollowerId = followerId, FollowedId = followingId });
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UnfollowAsync(int followerId, int followingId)
    {
        var follow = await db.Follows.FirstOrDefaultAsync(f =>
            f.FollowerId == followerId &&
            f.FollowedId == followingId
        );

        if (follow == null) return false;

        db.Follows.Remove(follow);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsFollowingAsync(int followerId, int followingId) =>
        await db.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowedId == followingId);

    public async Task<int> FollowersCountAsync(int userId) =>
        await db.Follows.CountAsync(f => f.FollowedId == userId);

    public async Task<int> FollowingCountAsync(int userId) =>
        await db.Follows.CountAsync(f => f.FollowerId == userId);
}