using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.User;

public class UsersResponse
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;
}
