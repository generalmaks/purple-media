using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PurpleMediaRest.DataAccess.Models;

public class ChatMessage
{
    [Key]
    public long Id { get; set; }
    
    [Required]
    public int SenderId { get; set; }
    [ForeignKey(nameof(SenderId)), JsonIgnore]
    public User Sender { get; set; }
    
    [Required]
    public int ReceiverId { get; set; }
    [ForeignKey(nameof(ReceiverId)), JsonIgnore]
    public User Receiver { get; set; }
    
    [Required]
    public string Content { get; set; }

    [Required]
    public DateTime MessageSent { get; set; } = DateTime.UtcNow;
    
    public bool IsRead { get; set; }
}