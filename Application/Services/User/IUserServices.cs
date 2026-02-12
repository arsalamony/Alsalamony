using Application.Common.Results;
using Application.Contracts.User;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.User;

public interface IUserServices
{
    Result<UserResponse> Get(int id);

    Result<IEnumerable<UsersResponse>> GetUsers();
    Result<UserResponse> Add(AddUserRequest request);

    Result Delete(int id);

    Result GetLocation(int userId, GetLocationRequest request);
}
