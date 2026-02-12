using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.User;
using Application.Contracts.UserProduct;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.User;

public class UserServices : IUserServices
{
    private readonly IUnitOfWork unitOfWork;

    public UserServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }


    public Result<IEnumerable<UsersResponse>> GetUsers()
    {
        var users = unitOfWork.UserRepository.GetAll();
        unitOfWork.Commit();

        var result = users.Select(u => new UsersResponse() { UserId = u.UserId, Name = u.Name });
        return Result.Success(result);
    }

    public Result<UserResponse> Get(int UserId)
    {
        
        var User = unitOfWork.UserRepository.Find(UserId);

        if (User is null) return Result.Failure<UserResponse>(UserErrors.UserNotFound);
        
        User.UserProducts = unitOfWork.UserProductRepository.GetAll(UserId);

        var response = new UserResponse
        {
            UserId = User.UserId,
            Name = User.Name,
            UserProducts = User.UserProducts.Select(e =>
            {
                e.Product = unitOfWork.ProductRepository.Find(e.ProductId);
                return new UserProductResponse
                {
                    ProductId = e.ProductId,
                    ProductName = e.Product.ProductName,
                    Quantity = e.Qty,
                };
            })
        };
        return Result.Success(response);
    }

    public Result<UserResponse> Add(AddUserRequest request)
    {

        var newUser = new Domain.Entities.User
        {
            Name = request.Name,
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role,
            DateOfLastLocation = null
        };

        if (!unitOfWork.UserRepository.Add(newUser)) 
        {
            unitOfWork.Rollback();
            return Result.Failure<UserResponse>(UserErrors.UserAddFailed);
        }
        
        var Products = unitOfWork.ProductRepository.GetAll();
        foreach (var product in Products)
        {
            var userProduct = new Domain.Entities.UserProduct
            {
                UserId = newUser.UserId,
                ProductId = product.ProductId,
                Qty = 0
            };
            if(!unitOfWork.UserProductRepository.Add(userProduct))
            {
                unitOfWork.Rollback();
                return Result.Failure<UserResponse>(UserErrors.UserAddFailed);
            }
            newUser.UserProducts.Add(userProduct);
        }



        var response = new UserResponse
        {
            UserId = newUser.UserId,
            Name = newUser.Name,
            UserProducts = Products.Select(e =>
            {
                return new UserProductResponse
                {
                    ProductId = e.ProductId,
                    ProductName = e.ProductName,
                    Quantity = 0,
                };
            })
        };


        unitOfWork.Commit();
        return Result.Success(response);
    }

    public Result Delete(int id)
    {
        var success = unitOfWork.UserRepository.Delete(id);
        unitOfWork.Commit();
        return success ? Result.Success() : Result.Failure(UserErrors.UserNotFound);
    }

    public Result GetLocation(int userId, GetLocationRequest request)
    {
        var user = unitOfWork.UserRepository.Find(userId);
        if (user == null)
            return Result.Failure(UserErrors.UserNotFound);

        user.Latitude = request.Latitude;
        user.Longitude = request.Longitude;
        user.DateOfLastLocation = DateTime.Now;

        return Result.Success();
    }
}
