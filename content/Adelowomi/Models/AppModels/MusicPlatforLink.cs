using System;
using System.ComponentModel.DataAnnotations;

namespace Adelowomi.Models.AppModels;

public class MusicPlatformLink : BaseEntity
{
    public int MusicReleaseId { get; set; }
    public MusicRelease MusicRelease { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string PlatformName { get; set; } = null!;
    
    [Required]
    [MaxLength(500)]
    public string Url { get; set; } = null!;
}