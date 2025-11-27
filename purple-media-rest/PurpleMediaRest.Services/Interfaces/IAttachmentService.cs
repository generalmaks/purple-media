using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;

namespace PurpleMediaRest.Services.Interfaces;

public interface IAttachmentService
{
    Task<TweetAttachment> AddAsync(int tweetId, string url, string contentType);
    Task<IEnumerable<TweetAttachment>> GetForTweetAsync(int tweetId);
}