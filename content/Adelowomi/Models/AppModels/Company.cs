using System;
using System.ComponentModel.DataAnnotations;

namespace Adelowomi.Models.AppModels;

public class Company : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [MaxLength(200)]
    public string? LogoUrl { get; set; }
    
    [MaxLength(200)]
    public string? WebsiteUrl { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    [MaxLength(100)]
    public string Position { get; set; } = null!;
    
    public List<ProjectHighlight> ProjectHighlights { get; set; } = new();
    public List<TechnologyUsed> Technologies { get; set; } = new();
}
