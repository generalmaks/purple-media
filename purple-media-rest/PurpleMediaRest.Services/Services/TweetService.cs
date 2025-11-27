using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.Services.Interfaces;
using TwitterClone.Data;

namespace PurpleMediaRest.Services.Services;

public class TweetService: ITweetService
{
    private readonly AppDbContext _db;

    public TweetService(AppDbContext db)
    {
        _db = db;
    }

    public Task<Tweet?> GetAsync(int id) =>
        _db.Tweets.Include(t => t.Attachments)
            .Include(t => t.Replies)
            .FirstOrDefaultAsync(t => t.Id == id);

    public Task<IEnumerable<Tweet>> GetUserTweetsAsync(int userId) =>
        Task.FromResult<IEnumerable<Tweet>>(
            _db.Tweets.Where(t => t.AuthorId == userId)
        );

    public async Task<Tweet> CreateAsync(int authorId, string text, int? parentTweetId = null)
    {
        var tweet = new Tweet
        {
            AuthorId = authorId,
            Content = text,
            ParentTweetId = parentTweetId,
            CreatedAt = DateTime.UtcNow
        };

        _db.Tweets.Add(tweet);
        await _db.SaveChangesAsync();

        return tweet;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var tweet = await _db.Tweets.FindAsync(id);
        if (tweet == null) return false;

        _db.Tweets.Remove(tweet);
        return await _db.SaveChangesAsync() > 0;
    }
}