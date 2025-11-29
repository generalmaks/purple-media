using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class LikeService : ILikeService
{
    private readonly AppDbContext _db;

    public LikeService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<bool> LikeAsync(int userId, int tweetId)
    {
        if (await IsLikedAsync(userId, tweetId))
            return false;

        _db.Likes.Add(new TweetLike() { UserId = userId, TweetId = tweetId });
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UnlikeAsync(int userId, int tweetId)
    {
        var like = await _db.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.TweetId == tweetId);
        if (like == null) return false;

        _db.Likes.Remove(like);
        return await _db.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsLikedAsync(int userId, int tweetId) =>
        await _db.Likes.AnyAsync(l => l.UserId == userId && l.TweetId == tweetId);

    public async Task<int> CountAsync(int tweetId) =>
        await _db.Likes.CountAsync(l => l.TweetId == tweetId);
}