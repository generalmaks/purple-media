using PurpleMediaRest.DataAccess.Models;
using PurpleMediaRest.Services.Dto.Chat;

namespace PurpleMediaRest.Services.Interfaces;

public interface IChatService
{
    Task<ChatMessage?> GetMessageByIdAsync(long messageId);
    Task<IEnumerable<ChatMessage>> GetMessagesFromChatAsync(
        int currentUserId,
        int otherUserId,
        int page,
        int pageSize);

    Task<IEnumerable<ChatInfoDto>> GetChatsInfo(int userId);
    Task MarkAsReadAsync(long messageId);

    Task<ChatMessage> SendMessageAsync(int senderId, int receiverId, string content);
    Task DeleteMessageAsync(long messageId);
}