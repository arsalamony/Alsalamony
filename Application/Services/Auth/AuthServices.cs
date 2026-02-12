using Alsalamony.Application.Common.Interfaces;
using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.Auth;
using Application.Contracts.User;
using Application.Services.User;
using System.Security.Cryptography;

namespace Application.Services.Auth;

public class AuthServices : IAuthServices
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IUserServices userServices;
    private readonly IJwtProvider jwtProvider;

    public AuthServices(IUnitOfWork unitOfWork, IUserServices userServices, IJwtProvider jwtProvider)
    {
        this.unitOfWork = unitOfWork;
        this.userServices = userServices;
        this.jwtProvider = jwtProvider;
    }


    public Result<AuthResponse> Login(LoginRequest loginRequest)
    {
        var user = unitOfWork.UserRepository.Find(loginRequest.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        

        var (tokenString, ExpireIn) = jwtProvider.GenerateToken(user);

        return Result.Success(new AuthResponse { Token = tokenString, Name = user.Name, Role = user.Role, UserId = user.UserId });
    }

    public Result Register(AddUserRequest request)
    {
        var re = userServices.Add(request); // commited itselt


        return re.IsSuccess ? Result.Success() : Result.Failure(UserErrors.UserAddFailed);
    }



    private static string GenerateRefreshToken()
    {
        var refreshToken = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(refreshToken);
    }
}
