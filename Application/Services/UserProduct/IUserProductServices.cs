using Application.Common.Results;
using Application.Contracts.UserProduct;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.UserProduct;

public interface IUserProductServices
{
    Result UpdateUserProductQuantity(UpdateUserProductQuantityRequest request);

    Result TransUserProductQuantity(int UserSenderId, UpdateUserProductQuantityRequest request);
}
