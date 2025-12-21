using System.ComponentModel.DataAnnotations;

namespace ThreadifyLab.Models;

public class ContactMessage
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255, ErrorMessage = "Name cannot exceed 255 characters")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters")]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(50, ErrorMessage = "Phone cannot exceed 50 characters")]
    public string Phone { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Subject is required")]
    [StringLength(255, ErrorMessage = "Subject cannot exceed 255 characters")]
    public string Subject { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Message is required")]
    public string Message { get; set; } = string.Empty;
    
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

