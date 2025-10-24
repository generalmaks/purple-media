using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using purple_media_rest.DTO;

namespace purple_media_rest.Models;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PostId { get; init; }
    [Required, Column(TypeName = "text"), MaxLength(150)]
    public string Content { get; init; } = "";
    [Required]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    
    [Required, Column(TypeName = "varchar(50)")]
    public string AuthorId { get; init; } = "";
    [ForeignKey(nameof(AuthorId))]
    public User? Author { get; init; }
    [Column(TypeName = "int")]
    public int? ParentPostId { get; init; }

    [ForeignKey(nameof(ParentPostId))]
    public Post? ParentPost { get; init; }

    public List<Post> ChildPosts { get; init; } = [];
    public List<User> LikedBy { get; init; } = [];
    public List<FileAttachment> Attachments { get; init; } = [];
    public GetPostDto ToGetPostDto(){
        var getPostDto = new GetPostDto{
            PostId = this.PostId,
            Content = this.Content,
            CreatedAt = this.CreatedAt,
            Author = this.AuthorId,
            AuthorsProfilePictureId = this.Author?.ProfilePictureId,
            ParentPost = this.ParentPostId,
            Responses = this.ChildPosts.Select(p => p.PostId).ToList(),
            LikedBy = this.LikedBy.Select(l => l.Username).ToList(),
        };
        return getPostDto;
    }
}