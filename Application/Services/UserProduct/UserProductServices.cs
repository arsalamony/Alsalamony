using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.UserProduct;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.UserProduct;

public class UserProductServices : IUserProductServices
{
    private readonly IUnitOfWork unitOfWork;

    public UserProductServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    // optimize later
    public Result TransUserProductQuantity(int UserSenderId, UpdateUserProductQuantityRequest request)
    {
        var userReceiverProduct = unitOfWork.UserProductRepository.GetAll(request.UserId)
            .FirstOrDefault(e => e.UserId == request.UserId && e.ProductId == request.ProductId);

        var userSenderProduct = unitOfWork.UserProductRepository.GetAll(UserSenderId)
            .FirstOrDefault(e => e.UserId == UserSenderId && e.ProductId == request.ProductId);


        if (userReceiverProduct == null || userSenderProduct == null)
            return Result.Failure(UserProductErrors.UserProductNotFound);

        var userSender = unitOfWork.UserRepository.Find(UserSenderId);
        var userReceiver = unitOfWork.UserRepository.Find(request.UserId);
        var product = unitOfWork.ProductRepository.Find(request.ProductId);
        string record = $"استلم {userReceiver.Name} {request.Qty} {product.ProductName} من {userSender.Name}";

        var systemRecord = new Domain.Entities.SystemRecord()
        {
            CreatedDate = DateTime.Now,
            Description = record,
            Finished = false,
            Level = 1
        };


        userReceiverProduct.Qty += request.Qty;
        userSenderProduct.Qty -= request.Qty;

        if (!unitOfWork.UserProductRepository.Update(userReceiverProduct) ||
            !unitOfWork.UserProductRepository.Update(userSenderProduct) ||
            !unitOfWork.SystemRecordRepository.Add(systemRecord)
            )
        {
            unitOfWork.Rollback();
            return Result.Failure(UserProductErrors.UserProductAddFailed);
        }

        unitOfWork.Commit();
        return Result.Success();
    }


    public Result UpdateUserProductQuantity(UpdateUserProductQuantityRequest request)
    {
        var userProduct = unitOfWork.UserProductRepository.GetAll(request.UserId)
            .FirstOrDefault(e => e.UserId == request.UserId && e.ProductId == request.ProductId);

        var user = unitOfWork.UserRepository.Find(request.UserId);
        var product = unitOfWork.ProductRepository.Find(request.ProductId);
        if (userProduct == null)
        {
            return Result.Failure(UserProductErrors.UserProductNotFound);
        }

        string t = request.Qty > 0 ? "تزويد" : "تنقيص";
        string re = $"تم {t} {request.Qty} {product.ProductName} ل {user.Name}";
        Domain.Entities.SystemRecord record = new Domain.Entities.SystemRecord() 
        {
        CreatedDate = DateTime.Now,
        Description = re,
        Finished = false,
        Level = 1,
        };

        // Update the quantity
        userProduct.Qty += request.Qty;

        if(!unitOfWork.UserProductRepository.Update(userProduct) || !unitOfWork.SystemRecordRepository.Add(record))
        {
            unitOfWork.Rollback();
            return Result.Failure(UserProductErrors.UserProductAddFailed);
        }

        unitOfWork.Commit();
        return Result.Success();
    }
}
