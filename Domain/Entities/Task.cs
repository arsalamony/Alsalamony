

using Domain.Enums;

namespace Domain.Entities;

public class Task
{
    public int TaskId { get; set; }

    public string Name { get; set; } = null!;

    public enTaskPriority Priority { get; set; } = enTaskPriority.Low;

    public enTaskStatus Status { get; set; } = enTaskStatus.New;

    public int AddressId { get; set; }

    public int? AssignedToUserId { get; set; } = null;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = null;
    public DateTime? CompletedAt { get; set; } = null;

    public int CreatedByUserId { get; set; }
    public int? CompletedByUserId { get; set; } = null;

    public string? Notes { get; set; } = null;

    public Address Address { get; set; } = null!;

    public User CreatedByUser { get; set; } = null!;
    public User? AssignedToUser { get; set; } = null;
    public User? CompletedByUser { get; set; } = null;

}
