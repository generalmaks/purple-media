using Microsoft.EntityFrameworkCore;
using PurpleMediaRest.DataAccess;
using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Chat;
using PurpleMediaRest.Services.Interfaces;

namespace PurpleMediaRest.Services.Services;

public class ChatService(AppDbContext db) : IChatService
{
    public async Task<ChatMessage?> GetMessageByIdAsync(long messageId)
    {
        return await db.ChatMessages.FindAsync(messageId);
    }

    public async Task<IEnumerable<ChatMessage>> GetMessagesFromChatAsync(
        int currentUserId, int otherUserId,
        int page, int pageSize)
    {
        return await db.ChatMessages.Where(m =>
                m.SenderId == currentUserId && m.ReceiverId == otherUserId ||
                m.SenderId == otherUserId && m.ReceiverId == currentUserId)
            .OrderByDescending(m => m.MessageSent)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IEnumerable<ChatInfoDto>> GetChatsInfo(int userId)
    {
        var messages = await db.ChatMessages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Include(m => m.Sender)
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .ToListAsync();

        var chats = messages
            .GroupBy(m => m.SenderId == userId ? m.ReceiverId : m.SenderId)
            .Select(g =>
            {
                var latestMessage = g.OrderByDescending(m => m.MessageSent).First();
                var otherUser = latestMessage.SenderId == userId
                    ? latestMessage.Receiver
                    : latestMessage.Sender;

                return new ChatInfoDto(
                    otherUser.Id,
                    otherUser.DisplayName,
                    latestMessage.Content,
                    latestMessage.MessageSent
                );
            })
            .ToList();

        return chats;
    }

    public async Task<ChatMessage> SendMessageAsync(int senderId, int receiverId, string content)
    {
        if (string.IsNullOrEmpty(content))
            throw new ArgumentException("Message content cannot be empty");

        if (senderId == receiverId)
            throw new ArgumentException("You cannot send messages to yourself");

        var message = new ChatMessage
        {
            SenderId = senderId,
            ReceiverId = receiverId,
            Content = content,
            MessageSent = DateTime.UtcNow,
            IsRead = false
        };

        await db.ChatMessages.AddAsync(message);
        await db.SaveChangesAsync();

        return message;
    }

    public async Task DeleteMessageAsync(long messageId)
    {
        var message = await db.ChatMessages.FindAsync(messageId);

        if (message != null)
        {
            db.ChatMessages.Remove(message);
            await db.SaveChangesAsync();
        }
    }
}