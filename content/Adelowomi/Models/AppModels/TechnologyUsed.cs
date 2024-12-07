using System;
using System.ComponentModel.DataAnnotations;

namespace Adelowomi.Models.AppModels;

public class TechnologyUsed : BaseEntity
{
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [MaxLength(100)]
    public string? Version { get; set; }
}
