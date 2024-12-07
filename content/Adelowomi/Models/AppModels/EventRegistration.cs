using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Adelowomi.Models.AppModels;

namespace Adelowomi.Models.AppModels;

public class EventRegistration : BaseEntity
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    
    public int EventTicketId { get; set; }
    public EventTicket EventTicket { get; set; } = null!;
    
    public int AttendeeId { get; set; }
    public Attendee Attendee { get; set; } = null!;
    
    [Required]
    public string RegistrationNumber { get; set; } = null!;
    
    public DateTime RegistrationDate { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal AmountPaid { get; set; }
}