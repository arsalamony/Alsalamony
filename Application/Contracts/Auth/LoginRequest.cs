using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Auth;

public class LoginRequest
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}
