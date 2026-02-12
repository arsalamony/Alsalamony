using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.User;

public record AddUserRequest
{
    public string Name { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
}
