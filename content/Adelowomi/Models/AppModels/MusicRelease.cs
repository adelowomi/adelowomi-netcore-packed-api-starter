using System;
using System.ComponentModel.DataAnnotations;

namespace Adelowomi.Models.AppModels;

public class MusicRelease : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    [MaxLength(200)]
    public string? CoverArtUrl { get; set; }
    
    [MaxLength(200)]
    public string? SpotifyUrl { get; set; }
    
    [MaxLength(200)]
    public string? AppleMusicUrl { get; set; }
    
    [MaxLength(200)]
    public string? SoundCloudUrl { get; set; }
    
    public List<MusicPlatformLink> PlatformLinks { get; set; } = new();
}

