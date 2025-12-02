namespace PurpleMediaRest.Services.Dto.Attachments;

public record FileUploadDto(Stream FileStream, string FileName, string ContentType, long FileSize);