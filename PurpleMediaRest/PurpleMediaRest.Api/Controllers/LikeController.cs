using Microsoft.AspNetCore.Mvc;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Api.Controllers;

[ApiController]
[Route("api/like")]
public class LikeController(ILikeService service) : ControllerBase
{
    [HttpPost("like/{userId:int}/{tweetId:int}")]
    public async Task<ActionResult<bool>> LikeAsync(int userId, int tweetId)
    {
        try
        {
            bool wasLiked = await service.LikeAsync(userId, tweetId);
            return Ok(wasLiked);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("unlike/{userId:int}/{tweetId:int}")]
    public async Task<ActionResult<bool>> UnlikeAsync(int userId, int tweetId)
    {
        try
        {
            bool wasLiked = await service.UnlikeAsync(userId, tweetId);
            return Ok(wasLiked);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("is-liked/{userId:int}/{tweetId:int}")]
    public async Task<ActionResult<bool>> IsLikedAsync(int userId, int tweetId)
    {
        try
        {
            bool isLiked = await service.IsLikedAsync(userId, tweetId);
            return Ok(isLiked);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("count-likes/{tweetId:int}")]
    public async Task<ActionResult<int>> CountLikesAsync(int tweetId)
    {
        try
        {
            int count = await service.CountAsync(tweetId);
            return Ok(count);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}