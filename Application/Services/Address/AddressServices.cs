using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.Address;


namespace Application.Services.Address;

public class AddressServices : IAddressServices
{
    private readonly IUnitOfWork unitOfWork;

    public AddressServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    // correct later
    public Result<IEnumerable<AddressResponse>> GetAll()
    {
        var arr = new[]
        {
            new AddressResponse{AddressId = 1, AddressName = "بني عدي"},
            new AddressResponse{AddressId = 2, AddressName = "العزيه"},
            new AddressResponse{AddressId = 3, AddressName = "العتامنه"},
            new AddressResponse{AddressId = 4, AddressName = "بني سند"},
            new AddressResponse{AddressId = 5, AddressName = "المندره"},
            new AddressResponse{AddressId = 6, AddressName = "عزبة عبد الباقي"}
        };
        
        return Result.Success<IEnumerable<AddressResponse>>(arr);
    }
}
