using Microsoft.AspNetCore.Mvc;
using purple_media_rest.PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Attachments;
using PurpleMediaRest.Services.Interfaces;

namespace purple_media_rest.Controllers;

[ApiController]
[Route("api/attachments")]
public class AttachmentController(IAttachmentService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromForm] int tweetId, [FromForm] IFormFile file)
    {
        try
        {
            await service.AddAsync(tweetId, new FileUploadDto(file.OpenReadStream(), file.FileName, file.ContentType, file.Length));
            return Created();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{tweetId:int}")]
    public async Task<ActionResult<IEnumerable<TweetAttachment>>> GetForTweetAsync(int tweetId)
    {
        try
        {
            var attachments = await service.GetForTweetAsync(tweetId);
            return Ok(attachments);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}