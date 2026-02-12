using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.Product;
using Domain.Entities;


namespace Application.Services.Product;

public class ProductServices : IProductServices
{
    private readonly IUnitOfWork unitOfWork;

    public ProductServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Result<IEnumerable<ProductViewResponse>> GetAll()
    {
        var re = unitOfWork.ProductRepository.GetAll();

        var re2 = re.Select(e => new ProductViewResponse()
        {
            Price = e.Price,
            ProductId = e.ProductId,
            ProductName = e.ProductName,
        });
        return Result.Success(re2);
    }

    public Result Add(AddProductRequest request)
    {
        var newProduct = new Domain.Entities.Product()
        {
            ProductName = request.ProductName,
            Price = request.Price
        };

        if (!unitOfWork.ProductRepository.Add(newProduct)) 
        {
            unitOfWork.Rollback();
            return Result.Failure(ProductErrors.ProductAddFailed);
        }

        var users = unitOfWork.UserRepository.GetAll();

        foreach (var user in users)
        {
            var newUserProduct = new Domain.Entities.UserProduct()
            {
                ProductId = newProduct.ProductId,
                UserId = user.UserId,
                Qty = 0
            };

            if (!unitOfWork.UserProductRepository.Add(newUserProduct)) 
            {
                unitOfWork.Rollback();
                return Result.Failure(ProductErrors.ProductAddFailed);
            }
        }

        unitOfWork.Commit();
        return Result.Success(newProduct);
    }
}
