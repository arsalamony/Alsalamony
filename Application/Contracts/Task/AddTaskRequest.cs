using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Task;

public class AddTaskRequest
{
    public string Name { get; set; } = null!;

    public enTaskPriority Priority { get; set; }

    public int AddressId { get; set; }

    public int? AssignedToUserId { get; set; }

    public string? Notes { get; set; }
}
