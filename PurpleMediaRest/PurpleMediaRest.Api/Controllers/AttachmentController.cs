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
                request.UserId,
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

    [HttpPost("pfp")]
    public async Task<ActionResult<GetAttachmentDto>> CreatePfp([FromForm] PfpCreateRequest request)
    {
        try
        {
            await service.AddPfpAsync(request.UserId,
                new FileUploadDto(
                    request.File.OpenReadStream(),
                    request.File.FileName,
                    request.File.ContentType,
                    request.File.Length));
            return Created();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("{tweetId:int}")]
    public async Task<ActionResult<IEnumerable<GetAttachmentDto>>> GetForTweetAsync(int tweetId)
    {
        try
        {
            var attachments = await service.GetForTweetAsync(tweetId);

            var dtos = attachments.Select(a => new GetAttachmentDto(
                a.Id,
                a.FileName,
                a.MediaType,
                $"/attachments/file/{a.Id}"
            ));

            return Ok(dtos);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("file/{fileId:int}")]
    public async Task<ActionResult<GetAttachmentDto>> GetFileAsync(int fileId)
    {
        try
        {
            var attachment = await service.GetAsync(fileId);
            var stream = new MemoryStream(attachment.Data);
            return File(stream, attachment.MediaType, attachment.FileName);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("pfp/{userId:int}")]
    public async Task<ActionResult<GetAttachmentDto>> GetForUsersPfpAsync(int userId)
    {
        try
        {
            var pfp = await service.GetForUsersPfpAsync(userId);
            if (pfp is null) return NotFound("Profile picture is not set");

            var dto = new GetAttachmentDto(
                pfp.Id,
                pfp.FileName,
                pfp.MediaType,
                $"/attachments/file/{pfp.Id}"
            );

            return Ok(dto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}