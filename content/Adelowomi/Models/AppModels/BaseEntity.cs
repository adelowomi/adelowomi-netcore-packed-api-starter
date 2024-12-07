using System;

namespace Adelowomi.Models.AppModels;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; }
    public bool IsActive { get; set; } = true;
}
