namespace PurpleMediaRest.Api.Dto.Chat;

public record SendMessageDto(int SenderId, int ReceiverId, string Content);