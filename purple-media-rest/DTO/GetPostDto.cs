namespace purple_media_rest.DTO;
public record GetPostDto {
    public int PostId { get; set ;}
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Author { get; set; }
    public int? AuthorsProfilePictureId { get; set; }
    public int? ParentPost { get; set; }
    public List<int> Responses { get; set; }
    public List<string> LikedBy { get; set; }
}