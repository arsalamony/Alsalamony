using Application.Common.Results;
using Application.Contracts.Product;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Product;

public interface IProductServices
{
    Result<IEnumerable<ProductViewResponse>> GetAll();

    Result Add(AddProductRequest request);
}
