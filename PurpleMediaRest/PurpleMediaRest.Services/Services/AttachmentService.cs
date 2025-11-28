using Microsoft.EntityFrameworkCore;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Attachments;
using PurpleMediaRest.Services.Interfaces;
using TwitterClone.Data;

namespace PurpleMediaRest.Services.Services;

public class AttachmentService : IAttachmentService
{
    private const int MaxFileSize = 10 * 1024 * 1024;
    private readonly AppDbContext _db;

    public AttachmentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<TweetAttachment> AddAsync(int tweetId, FileUploadDto fileUpload)
    {
        if (fileUpload.fileStream is null || fileUpload.fileSize == 0)
            throw new ArgumentException("No file provided");
        if (fileUpload.fileSize > MaxFileSize)
            throw new ArgumentException($"File size exceeds maximum allowed size of {MaxFileSize / 1024 / 1024}.");

        var tweetExists = await _db.Tweets.AnyAsync(t => t.Id == tweetId);
        if (!tweetExists)
            throw new ArgumentException("Tweet doesnt exist.");
        
        var extension = Path.GetExtension(fileUpload.fileName).ToLowerInvariant();
        var fileName = $"{fileUpload.fileName}-{Guid.NewGuid()}{extension}";

        byte[] fileData;
        using (var ms = new MemoryStream())
        {
            await fileUpload.fileStream.CopyToAsync(ms);
            fileData = ms.ToArray();
        }

        var attachment = new TweetAttachment
        {
            TweetId = tweetId,
            Data = fileData,
            MediaType = fileUpload.contentType,
            FileName = fileName
        };

        await _db.Attachments.AddAsync(attachment);
        await _db.SaveChangesAsync();

        return attachment;
    }

    public async Task<IEnumerable<TweetAttachment>> GetForTweetAsync(int tweetId) =>
        await Task.FromResult<IEnumerable<TweetAttachment>>(
            _db.Attachments.Where(a => a.TweetId == tweetId)
        );
}