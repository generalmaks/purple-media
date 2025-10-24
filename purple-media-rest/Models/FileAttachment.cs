using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace purple_media_rest.Models;

public class FileAttachment
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FileId { get; init; }
    [MaxLength(50)]
    public required string DataFormat { get; init; }

    [Required] public byte[] Data { get; init; } = [];
    [MaxLength(80)]
    public required string Name { get; init; }
    
    public int? PostId { get; init; }
    [ForeignKey(nameof(PostId))]
    public Post? Post { get; init; }
    
    public required string OwnerId { get; init; }
    [ForeignKey(nameof(OwnerId))]
    public required User Owner { get; init; }
}