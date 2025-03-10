using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace purple_media_rest.Models
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [Required]
        public int PostId { get; set; }
        [ForeignKey(nameof(PostId))]
        public Post? Post { get; set; }

        [Required, StringLength(255)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public User? Author { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public CommentDTO ToDTO(){
            return new CommentDTO{
                CommentId = this.CommentId,
                PostId = this.PostId,
                Content = this.Content,
                Author = this.Author.Username,
                CreatedAt = this.CreatedAt
            };
        }
    }
}