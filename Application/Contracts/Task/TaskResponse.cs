using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Task;

public class TaskResponse
{
    public int TaskId { get; set; }

    public string Name { get; set; } = null!;

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string AddressName { get; set; } = null!;

    public int? AssignedToUserId { get; set; }
    public string? AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedByUserId { get; set; }
    public string CreatedBy { get; set; } = null!;

    public DateTime? UpdatedAt { get; set; }

    public DateTime?  CompletedAt { get; set; }

    public int? CompletedByUserId { get; set; }
    public string? CompletedBy { get; set; }

    public string? Notes { get; set; }
}
