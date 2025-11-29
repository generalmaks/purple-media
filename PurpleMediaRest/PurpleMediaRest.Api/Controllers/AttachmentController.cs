using Microsoft.AspNetCore.Mvc;
using PurpleMediaRest.Api.Dto.Attachments;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Attachments;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Api.Controllers;

[ApiController]
[Route("api/attachments")]
public class AttachmentController(IAttachmentService service) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromForm] AttachmentCreateRequest request)
    {
        try
        {
            await service.AddAsync(request.TweetId,
                new FileUploadDto(
                    request.File.OpenReadStream(),
                    request.File.FileName,
                    request.File.ContentType,
                    request.File.Length
                ));
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