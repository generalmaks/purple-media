namespace PurpleMediaRest.Api.Dto.Attachments;

public record AttachmentCreateRequest(int? TweetId, int? UserId, IFormFile File);