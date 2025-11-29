namespace PurpleMediaRest.Api.Dto.Attachments;

public record AttachmentCreateRequest(int TweetId, IFormFile File);