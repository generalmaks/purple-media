using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Attachments;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class AttachmentService(AppDbContext db) : IAttachmentService
{
    private const int MaxFileSize = 2 * 1024 * 1024;

    public async Task<TweetAttachment> AddAsync(int? tweetId, int? userPfpId, FileUploadDto fileUpload)
    {
        if (fileUpload.FileStream is null || fileUpload.FileSize == 0)
            throw new ArgumentException("No file provided");
        if (fileUpload.FileSize > MaxFileSize)
            throw new ArgumentException($"File size exceeds maximum allowed size of {MaxFileSize / 1024 / 1024}MB.");
        if ((tweetId is null && userPfpId is null) || (tweetId is not null && userPfpId is not null))
            throw new ArgumentException("You can attach file only to tweet or users profile picture");
        if (userPfpId is not null && !fileUpload.ContentType.StartsWith("image/"))
            throw new ArgumentException("You can only upload images as users profile picture");

        if(tweetId is not null)
        {
            var tweetExists = await db.Tweets.AnyAsync(t => t.Id == tweetId);
            if (!tweetExists)
                throw new ArgumentException("Tweet doesnt exist.");
        }

        var extension = Path.GetExtension(fileUpload.FileName).ToLowerInvariant();
        var fileName = $"{fileUpload.FileName}-{Guid.NewGuid()}{extension}";

        byte[] fileData;
        using (var ms = new MemoryStream())
        {
            await fileUpload.FileStream.CopyToAsync(ms);
            fileData = ms.ToArray();
        }

        var attachment = new TweetAttachment
        {
            TweetId = tweetId,
            UserPfpId = userPfpId,
            Data = fileData,
            MediaType = fileUpload.ContentType,
            FileName = fileName
        };

        await db.Attachments.AddAsync(attachment);
        await db.SaveChangesAsync();

        return attachment;
    }

    public async Task<TweetAttachment> AddPfpAsync(int userPfpId, FileUploadDto fileUpload)
    {
        if (fileUpload.FileStream is null || fileUpload.FileSize == 0)
            throw new ArgumentException("No file provided");
        if (fileUpload.FileSize > MaxFileSize)
            throw new ArgumentException($"File size exceeds maximum allowed size of {MaxFileSize / 1024 / 1024}MB.");
        if (!fileUpload.ContentType.StartsWith("image/"))
            throw new ArgumentException("You can only upload images as users profile picture");
        
        var extension = Path.GetExtension(fileUpload.FileName).ToLowerInvariant();
        var fileName = $"{fileUpload.FileName}-{Guid.NewGuid()}{extension}";

        byte[] fileData;
        using (var ms = new MemoryStream())
        {
            await fileUpload.FileStream.CopyToAsync(ms);
            fileData = ms.ToArray();
        }

        var attachment = new TweetAttachment
        {
            TweetId = null,
            UserPfpId = userPfpId,
            Data = fileData,
            MediaType = fileUpload.ContentType,
            FileName = fileName
        };

        var alreadyExistingUsersPfp = await db.Attachments.FirstOrDefaultAsync(a => a.UserPfpId == userPfpId);
        if (alreadyExistingUsersPfp is not null)
            db.Attachments.Remove(alreadyExistingUsersPfp);
        await db.Attachments.AddAsync(attachment);
        await db.SaveChangesAsync();

        return attachment;
    }

    public async Task<IEnumerable<TweetAttachment>> GetForTweetAsync(int tweetId) =>
        await Task.FromResult<IEnumerable<TweetAttachment>>(
            db.Attachments.Where(a => a.TweetId == tweetId)
        );

    public async Task<TweetAttachment?> GetForUsersPfpAsync(int userId) =>
        await db.Attachments.FirstOrDefaultAsync(a => a.UserPfpId == userId);
    public async Task<TweetAttachment> GetAsync(int fileId) =>
        await db.Attachments.FindAsync(fileId) ?? throw new KeyNotFoundException("File not found.");
}