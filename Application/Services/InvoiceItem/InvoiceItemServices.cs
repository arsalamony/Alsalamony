using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.InvoiceItem;


namespace Application.Services.InvoiceItem;

public class InvoiceItemServices : IInvoiceItemSerivces
{
    private readonly IUnitOfWork unitOfWork;

    public InvoiceItemServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<InvoiceItemResponse>>> GetInvoiceItemsByInvoiceId(int invoiceId)
    {
        await unitOfWork.OpenAsync(true);
        var invoiceItems = await unitOfWork.InvoiceItemRepository.GetInvoiceItems(invoiceId);

        var invoiceItemResponses = invoiceItems.Select(i =>
        {
            i.Product = unitOfWork.ProductRepository.Find(i.ProductId).GetAwaiter().GetResult();
            return new InvoiceItemResponse
            {
                ProductName = i.Product.ProductName,
                Qty = i.Qty,
                PricePerUnit = i.PricePerUnit,
                IsGift = i.IsGift

            };
        }).ToList();

        return Result.Success<IEnumerable<InvoiceItemResponse>>(invoiceItemResponses);
    }
}
