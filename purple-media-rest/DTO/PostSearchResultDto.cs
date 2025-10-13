namespace purple_media_rest.DTO;

public class PostSearchResultDto
{
    public GetPostDto Post { get; set; }
    public int[] Indices { get; set; }
}