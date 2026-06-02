using System.ComponentModel.DataAnnotations;

namespace ThreadifyLab.Models;

public class User
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(255)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(255)]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; } = string.Empty;
    
    
    [StringLength(255)]
    public string PasswordHash { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? Phone { get; set; }
    
    public bool IsEmailVerified { get; set; }
    
    [StringLength(255)]
    public string? EmailVerificationToken { get; set; }
    
    public DateTime? EmailVerificationTokenExpiry { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime? LastLoginAt { get; set; }
    
    public bool IsActive { get; set; }
}












