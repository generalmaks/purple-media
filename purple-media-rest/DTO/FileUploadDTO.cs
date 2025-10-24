using System.ComponentModel.DataAnnotations;

namespace purple_media_rest.DTO;

public class FileUploadDTO
{
    [Required]
    public IFormFile File { get; set; } = null!;

    public int? PostId { get; set; }

    [Required]
    public string OwnerUserName { get; set; } = "";
}