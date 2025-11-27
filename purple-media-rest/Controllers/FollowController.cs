using Microsoft.AspNetCore.Mvc;
using PurpleMediaRest.Services.Interfaces;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/follow")]
public class FollowController(IFollowService service) : ControllerBase
{
    [HttpPost("follow/{followerId:int}/{followingId:int}")]
    public async Task<ActionResult<bool>> FollowAsync(int followerId, int followingId)
    {
        try
        {
            bool isFollowing = await service.FollowAsync(followerId, followingId);
            return Ok(isFollowing);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("unfollow/{followerId:int}/{followingId:int}")]
    public async Task<ActionResult<bool>> UnfollowAsync(int followerId, int followingId)
    {
        try
        {
            bool isFollowing = await service.UnfollowAsync(followerId, followingId);
            return Ok(isFollowing);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("following/{followerId:int}/{followingId:int}")]
    public async Task<ActionResult<bool>> IsFollowing(int followerId, int followingId)
    {
        try
        {
            bool isFollowing = await service.IsFollowingAsync(followerId, followingId);
            return Ok(isFollowing);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("count-followers/{userId:int}")]
    public async Task<ActionResult<int>> CountFollowers(int userId)
    {
        try
        {
            int count = await service.FollowersCountAsync(userId);
            return Ok(count);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("count-following/{userId:int}")]
    public async Task<ActionResult<int>> CountFollowing(int userId)
    {
        try
        {
            int count = await service.FollowingCountAsync(userId);
            return Ok(count);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}