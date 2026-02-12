using Application.Common.Results;
using Application.Contracts.Auth;
using Application.Contracts.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Auth;

public interface IAuthServices
{
    Result<AuthResponse> Login(LoginRequest loginRequest);

    Result Register(AddUserRequest Request);

}
