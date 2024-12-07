using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Adelowomi.Models.AppModels;

public class EventTicket : BaseEntity
{
    public int EventId { get; set; }
    public Event Event { get; set; } = null!;
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    public int? Quantity { get; set; }
    public int? AvailableQuantity { get; set; }
    
    public DateTime? SaleStartDate { get; set; }
    public DateTime? SaleEndDate { get; set; }
}
