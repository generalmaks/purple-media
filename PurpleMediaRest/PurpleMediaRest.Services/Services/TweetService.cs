using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Tweet;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class TweetService(AppDbContext db) : ITweetService
{
    public async Task<TweetDto?> GetAsync(int id)
    {
        var tweet = await db.Tweets.Include(t => t.Attachments)
            .Include(t => t.Replies)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (tweet is null)
            throw new KeyNotFoundException("Tweet not found");

        return new TweetDto(tweet.Id, tweet.AuthorId, tweet.Content, tweet.ParentTweetId, tweet.CreatedAt);
    }

    public async Task<IEnumerable<TweetDto>> GetLatestAsync(int page, int pageSize)
    {
        var tweets = await db.Tweets.Include(t => t.Attachments)
            .Include(t => t.Replies)
            .Where(t => t.ParentTweetId == null)
            .OrderByDescending(t => t.CreatedAt)
            .Skip(page * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return tweets.Select(t => new TweetDto(t.Id, t.AuthorId, t.Content, t.ParentTweetId, t.CreatedAt));
    }

    public async Task<IEnumerable<TweetDto>> GetUserTweetsAsync(int userId)
    {
        var tweets = await db.Tweets.Where(t => t.AuthorId == userId).ToListAsync();

        return tweets.Select(t => new TweetDto(t.Id, t.AuthorId, t.Content, t.ParentTweetId, t.CreatedAt));
    }

    public async Task<IEnumerable<TweetDto>> GetResponsesToTweetAsync(int tweetId)
    {
        if (await db.Tweets.FindAsync(tweetId) is null)
            throw new KeyNotFoundException("Tweet was not found");
        
        return await db.Tweets
            .Where(t => t.ParentTweetId == tweetId)
            .Select(t => new TweetDto(t.Id, t.AuthorId, t.Content, t.ParentTweetId, t.CreatedAt))
            .ToListAsync();
    }

    public async Task<TweetDto> CreateAsync(int authorId, string content, int? parentTweetId = null)
    {
        if (parentTweetId is not null && await db.Tweets.FindAsync(parentTweetId) is null)
            throw new KeyNotFoundException("Parent tweet not found.");
        
        var tweet = new Tweet
        {
            AuthorId = authorId,
            Content = content,
            ParentTweetId = parentTweetId,
            CreatedAt = DateTime.UtcNow
        };

        db.Tweets.Add(tweet);
        await db.SaveChangesAsync();

        var tweetDto = new TweetDto(tweet.Id, authorId, content, parentTweetId, DateTime.UtcNow);

        return tweetDto;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tweet = await db.Tweets.FindAsync(id);
        if (tweet == null) return false;

        db.Tweets.Remove(tweet);
        return await db.SaveChangesAsync() > 0;
    }
}