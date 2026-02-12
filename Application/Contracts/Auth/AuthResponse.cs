using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Auth;

public class AuthResponse
{
    public string Token { get; set; } = null!;

    public int UserId { get; set; }

    public string Role { get; set; } = null!;

    public string Name { get; set; } = null!;
}
