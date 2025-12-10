using Microsoft.AspNetCore.Mvc;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Tweet;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Api.Controllers;

[ApiController]
[Route("/api/tweets")]
public class TweetController(ITweetService service) : ControllerBase
{
    [HttpGet("latest/{page:int}/{pageSize:int}")]
    public async Task<ActionResult<IEnumerable<TweetDto>>> GetLatest(int page, int pageSize)
    {
        try
        {
            var latest = await service.GetLatestAsync(page, pageSize);
            return Ok(latest);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{tweetId:int}")]
    public async Task<ActionResult<TweetDto?>> GetAsync(int tweetId)
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
    public async Task<ActionResult<IEnumerable<TweetDto>>> GetUserTweetsAsync(int userId)
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

    [HttpGet("responses/{tweetId:int}")]
    public async Task<ActionResult<IEnumerable<TweetDto>>> GetResponsesToTweetAsync(int tweetId)
    {
        try
        {
            var responses = await service.GetResponsesToTweetAsync(tweetId);
            return Ok(responses);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    [HttpPost("{authorId:int}/{content}")]
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