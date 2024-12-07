using System;
using System.ComponentModel.DataAnnotations;

namespace Adelowomi.Models.AppModels;

public class Event : BaseEntity
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = null!;
    
    [Required]
    [MaxLength(2000)]
    public string Description { get; set; } = null!;
    
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    [MaxLength(200)]
    public string? Venue { get; set; }
    
    [MaxLength(500)]
    public string? Address { get; set; }
    
    public int? MaxAttendees { get; set; }
    
    [MaxLength(200)]
    public string? BannerImageUrl { get; set; }
    
    public bool IsVirtual { get; set; }
    
    [MaxLength(500)]
    public string? VirtualMeetingUrl { get; set; }
    
    public List<EventTicket> Tickets { get; set; } = new();
    public List<EventRegistration> Registrations { get; set; } = new();
}