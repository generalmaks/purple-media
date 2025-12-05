namespace PurpleMediaRest.Services.Dto.Chat;

public record ChatInfoDto(int OtherUserId, string OtherUserUsername, string LastMessageContent, DateTime LastMessageSentTime);