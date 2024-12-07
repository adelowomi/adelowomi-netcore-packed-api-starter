using System;
using System.ComponentModel.DataAnnotations;

namespace Adelowomi.Models.AppModels;

public class Attendee : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = null!;
    
    [MaxLength(20)]
    public string? PhoneNumber { get; set; }
    
    public List<EventRegistration> Registrations { get; set; } = new();
}
