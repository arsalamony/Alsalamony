using Application.Common.Results;
using Application.Contracts.Auth;
using Application.Contracts.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Auth;

public interface IAuthServices
{
    Task<Result<AuthResponse>> Login(LoginRequest loginRequest);

    Task<Result> Register(AddUserRequest Request);

}
