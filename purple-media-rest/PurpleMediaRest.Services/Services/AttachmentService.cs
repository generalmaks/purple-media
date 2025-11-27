using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Interfaces;
using TwitterClone.Data;

namespace PurpleMediaRest.Services.Services;

public class AttachmentService : IAttachmentService
{
    private readonly AppDbContext _db;

    public AttachmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TweetAttachment> AddAsync(int tweetId, string url, string contentType)
    {
        var attachment = new TweetAttachment
        {
            TweetId = tweetId,
            Url = url,
            MediaType = contentType
        };

        _db.Attachments.Add(attachment);
        await _db.SaveChangesAsync();

        return attachment;
    }

    public async Task<IEnumerable<TweetAttachment>> GetForTweetAsync(int tweetId) =>
        await Task.FromResult<IEnumerable<TweetAttachment>>(
            _db.Attachments.Where(a => a.TweetId == tweetId)
        );
}