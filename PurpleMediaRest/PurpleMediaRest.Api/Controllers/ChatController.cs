using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PurpleMediaRest.Api.Dto.Chat;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Chat;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Api.Controllers;

[ApiController]
[Route("api/chatMessages")]
public class ChatController(IChatService service) : ControllerBase
{
    [HttpGet("{messageId:long}")]
    public async Task<ActionResult<ChatMessage>> GetMessageByIdAsync(long messageId)
    {
        try
        {
            var message = await service.GetMessageByIdAsync(messageId);
            return Ok(message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("thread/{currentUserId:int}/{otherUserId:int}/{page:int}/{pageSize:int}")]
    public async Task<ActionResult<IEnumerable<ChatMessage>>> GetMessagesFromChatAsync(
        int currentUserId,
        int otherUserId,
        int page,
        int pageSize)
    {
        try
        {
            var messages = await service.GetMessagesFromChatAsync(
                currentUserId, otherUserId, page, pageSize);

            return Ok(messages);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpGet("chats-info")]
    public async Task<ActionResult<IEnumerable<ChatInfoDto>>> GetChatsInfoAsync()
    {
        try
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdString, out int userId))
                return Unauthorized("Invalid user ID in token");

            var messages = await service.GetChatsInfo(userId);

            return Ok(messages);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<ChatMessage>> SendMessageAsync([FromBody] SendMessageDto request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Content is required");

            var sentMessage = await service.SendMessageAsync(
                request.SenderId, 
                request.ReceiverId, 
                request.Content
            );

            return Created();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete("{messageId:int}")]
    public async Task<IActionResult> DeleteMessageAsync(int messageId)
    {
        try
        {
            await service.DeleteMessageAsync(messageId);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}