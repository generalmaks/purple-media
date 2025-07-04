using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using purple_media_rest.DTO;

namespace purple_media_rest.Models;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PostId { get; set; }
    [Required, Column(TypeName = "text")]
    public string Content { get; set; } = "";
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Required, Column(TypeName = "varchar(50)")]
    public string AuthorId { get; set; } = "";
    [ForeignKey(nameof(AuthorId))]
    public User? Author { get; set; }
    [Column(TypeName = "int")]
    public int? ParentPostId { get; set; }

    [ForeignKey(nameof(ParentPostId))]
    public Post? ParentPost { get; set; }

    public List<Post> ChildPosts { get; set; } = [];
    public List<User> LikedBy { get; set; } = [];
    public GetPostDto ToGetPostDto(){
        var getPostDto = new GetPostDto{
            PostId = this.PostId,
            Content = this.Content,
            CreatedAt = this.CreatedAt,
            Author = this.AuthorId,
            AuthorProfilePicturePath = this.Author?.ProfilePicturePath,
            ParentPost = this.ParentPostId,
            Responses = this.ChildPosts.Select(p => p.PostId).ToList(),
            LikedBy = this.LikedBy.Select(l => l.Username).ToList(),
        };
        return getPostDto;
    }
}