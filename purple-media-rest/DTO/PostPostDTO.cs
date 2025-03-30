namespace purple_media_rest.DTO;
public record PostPostDTO(){
    public string AuthorId { get; set; } = "";
    public string Content { get; set; } = "";
    public int? ParentPostId { get; set; } = null;
}