using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Attachments;

namespace PurpleMediaRest.Services.Interfaces;

public interface IAttachmentService
{
    Task<TweetAttachment> AddAsync(int? tweetId, int? userPfpId, FileUploadDto fileUploadDto);
    Task<TweetAttachment> AddPfpAsync(int userPfpId, FileUploadDto fileUpload);
    Task<IEnumerable<TweetAttachment>> GetForTweetAsync(int tweetId);
    Task<TweetAttachment?> GetForUsersPfpAsync(int userId);
    Task<TweetAttachment> GetAsync(int fileId);
}