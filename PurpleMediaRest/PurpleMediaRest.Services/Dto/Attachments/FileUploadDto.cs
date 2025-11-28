namespace PurpleMediaRest.Services.Dto.Attachments;

public record FileUploadDto(Stream fileStream, string fileName, string contentType, long fileSize);