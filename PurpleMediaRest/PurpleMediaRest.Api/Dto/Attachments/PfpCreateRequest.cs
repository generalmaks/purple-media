namespace PurpleMediaRest.Api.Dto.Attachments;

public record PfpCreateRequest(int UserId, IFormFile File);