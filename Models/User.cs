using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace purple_media_rest.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string Username { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}