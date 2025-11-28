using Microsoft.AspNetCore.Mvc;
using PurpleMediaRest.Services.Interfaces;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("/api/tweets")]
public class TweetController(ITweetService service) : ControllerBase
{
    [HttpGet("{tweetId:int}")]
    public async Task<ActionResult<Tweet?>> GetAsync(int tweetId)
    {
        try
        {
            var tweet = await service.GetAsync(tweetId);
            return Ok(tweet);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("from-user/{userId:int}")]
    public async Task<ActionResult<IEnumerable<Tweet>>> GetUserTweetsAsync(int userId)
    {
        try
        {
            var tweets = await service.GetUserTweetsAsync(userId);
            return Ok(tweets);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{authorId:int}/{content}/{parentId:int?}")]
    public async Task<ActionResult> CreateAsync(int authorId, string content, int? parentId)
    {
        try
        {
            var createdTweet = await service.CreateAsync(authorId, content, parentId);
            return CreatedAtAction("Create", createdTweet, createdTweet);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{tweetId:int}")]
    public async Task<ActionResult<bool>> DeleteAsync(int tweetId)
    {
        try
        {
            var isDeleted = await service.DeleteAsync(tweetId);
            return Ok(isDeleted);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}