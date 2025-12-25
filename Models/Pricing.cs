namespace ThreadifyLab.Models;

public class Pricing
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty; // Normal, Complex, etc.
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // EmbroideryDigitizing, VectorArt
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

