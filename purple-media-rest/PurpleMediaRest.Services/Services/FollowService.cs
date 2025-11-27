using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Interfaces;
using TwitterClone.Data;

namespace PurpleMediaRest.Services.Services;

public class FollowService : IFollowService
{
    private readonly AppDbContext _db;

    public FollowService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<bool> FollowAsync(int followerId, int followingId)
    {
        if (followerId == followingId) return false;
        if (await IsFollowingAsync(followerId, followingId)) return false;

        _db.Follows.Add(new Follow { FollowerId = followerId, FollowedId = followingId });
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UnfollowAsync(int followerId, int followingId)
    {
        var follow = await _db.Follows.FirstOrDefaultAsync(f =>
            f.FollowerId == followerId &&
            f.FollowedId == followingId
        );

        if (follow == null) return false;

        _db.Follows.Remove(follow);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsFollowingAsync(int followerId, int followingId) =>
        await _db.Follows.AnyAsync(f => f.FollowerId == followerId && f.FollowedId == followingId);

    public async Task<int> FollowersCountAsync(int userId) =>
        await _db.Follows.CountAsync(f => f.FollowedId == userId);

    public async Task<int> FollowingCountAsync(int userId) =>
        await _db.Follows.CountAsync(f => f.FollowerId == userId);
}