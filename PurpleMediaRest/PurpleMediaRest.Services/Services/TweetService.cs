using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class TweetService(AppDbContext db) : ITweetService
{
    public Task<Tweet?> GetAsync(int id) =>
        db.Tweets.Include(t => t.Attachments)
            .Include(t => t.Replies)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<Tweet>> GetLatestAsync(int page, int pageSize)
    {
        return await db.Tweets.Include(t => t.Attachments)
            .Include(t => t.Replies)
            .OrderByDescending(t => t.CreatedAt)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public Task<IEnumerable<Tweet>> GetUserTweetsAsync(int userId) =>
        Task.FromResult<IEnumerable<Tweet>>(
            db.Tweets.Where(t => t.AuthorId == userId)
        );

    public async Task<Tweet> CreateAsync(int authorId, string content, int? parentTweetId = null)
    {
        var tweet = new Tweet
        {
            AuthorId = authorId,
            Content = content,
            ParentTweetId = parentTweetId,
            CreatedAt = DateTime.UtcNow
        };

        db.Tweets.Add(tweet);
        await db.SaveChangesAsync();

        return tweet;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tweet = await db.Tweets.FindAsync(id);
        if (tweet == null) return false;

        db.Tweets.Remove(tweet);
        return await db.SaveChangesAsync() > 0;
    }
}