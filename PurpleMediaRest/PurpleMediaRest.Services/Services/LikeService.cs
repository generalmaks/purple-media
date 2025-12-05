using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class LikeService(AppDbContext db) : ILikeService
{
    public async Task<bool> LikeAsync(int userId, int tweetId)
    {
        if (await IsLikedAsync(userId, tweetId))
            return false;

        db.Likes.Add(new TweetLike() { UserId = userId, TweetId = tweetId });
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> UnlikeAsync(int userId, int tweetId)
    {
        var like = await db.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.TweetId == tweetId);
        if (like == null) return false;

        db.Likes.Remove(like);
        return await db.SaveChangesAsync() > 0;
    }

    public async Task<bool> IsLikedAsync(int userId, int tweetId) =>
        await db.Likes.AnyAsync(l => l.UserId == userId && l.TweetId == tweetId);

    public async Task<int> CountAsync(int tweetId) =>
        await db.Likes.CountAsync(l => l.TweetId == tweetId);
}