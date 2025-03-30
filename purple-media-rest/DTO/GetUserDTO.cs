namespace purple_media_rest.DTO;
public record GetUserDTO(){
    public string Username { get; set; } = "";
    public string ProfilePicturePath { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
}