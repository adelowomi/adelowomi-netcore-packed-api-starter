using System;
using System.ComponentModel.DataAnnotations;
using Adelowomi.Models.AppModels;

namespace Adelowomi.Models;

public class ProjectHighlight : BaseEntity
{
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
}
