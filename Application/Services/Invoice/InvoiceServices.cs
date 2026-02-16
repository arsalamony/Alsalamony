using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.Invoice;
using Application.Contracts.InvoiceItem;
using Domain.Entities;

namespace Application.Services.Invoice;

public class InvoiceServices : IInvoiceServices
{
    private readonly IUnitOfWork unitOfWork;

    public InvoiceServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Result<IEnumerable<InvoiceResponse>> GetAllUnpayed(int customerId)
    {
        var rows = unitOfWork.InvoiceRepository.GetAllUnpayedRowsByCustomerId(customerId);

        unitOfWork.Commit();

        var dict = new Dictionary<int, InvoiceResponse>();
        var itemsDict = new Dictionary<int, List<InvoiceItemResponse>>();

        foreach (var r in rows)
        {
            if (!dict.ContainsKey(r.InvoiceId))
            {
                dict[r.InvoiceId] = new InvoiceResponse
                {
                    InvoiceId = r.InvoiceId,
                    CustomerName = r.CustomerName,
                    InvoiceDate = r.InvoiceDate,
                    TotalAmount = r.TotalAmount,
                    AmountPaid = r.AmountPaid,
                    RemainingAmount = r.RemainingAmount,
                    CreatedBy = r.CreatedBy,
                    Notes = r.Notes
                };

                itemsDict[r.InvoiceId] = new List<InvoiceItemResponse>();
            }

            // لو مفيش Items (فاتورة بدون عناصر) يبقى ProductName = null
            if (r.ProductName != null)
            {
                itemsDict[r.InvoiceId].Add(new InvoiceItemResponse
                {
                    ProductName = r.ProductName,
                    Qty = r.Qty ?? 0,
                    PricePerUnit = r.PricePerUnit ?? 0m,
                    IsGift = r.IsGift ?? false
                });
            }
        }

        // ربط الـ items لكل فاتورة
        foreach (var kv in dict)
        {
            kv.Value.InvoiceItems = itemsDict[kv.Key];
        }


        var result = dict.Values.ToList();
        //    .OrderByDescending(x => x.InvoiceDate)
        //    .ThenBy(x => x.InvoiceId)
        //    .ToList();

        return Result.Success<IEnumerable<InvoiceResponse>>(result);
    }

    public Result<InvoiceResponse> Get(int invoiceId)
    {
        var invoice = unitOfWork.InvoiceRepository.Find(invoiceId);

        if (invoice is null)
            return Result.Failure<InvoiceResponse>(InvoiceErrors.InvoiceNotFound);

        invoice.CreatedByUser = unitOfWork.UserRepository.Find(invoice.CreatedByUserId);

        if(invoice.CustomerId != null)
            invoice.Customer = unitOfWork.CustomerRepository.Find((int)invoice.CustomerId);

        var response = new InvoiceResponse
        {
            InvoiceId = invoice.InvoiceId,
            CustomerName = invoice.Customer?.CustomerName?? "مجهول",
            InvoiceDate = invoice.InvoiceDate,
            TotalAmount = invoice.TotalAmount,
            AmountPaid = invoice.AmountPaid,
            RemainingAmount = invoice.RemainingAmount,
            CreatedBy = invoice.CreatedByUser?.Name ?? "مجهول",
            Notes = invoice.Notes
        };

        return Result.Success(response);
    }

    public Result<InvoiceResponse> Add(int UserId, AddInvoiceRequest request)
    {
        
        var User = unitOfWork.UserRepository.Find(UserId);
        if (User is null)
            return Result.Failure<InvoiceResponse>(UserErrors.UserNotFound);
        User.UserProducts = unitOfWork.UserProductRepository.GetAll(UserId).ToList();


        // Map Invoice
        Domain.Entities.Invoice invoice = new Domain.Entities.Invoice
        {
            CustomerId = request.CustomerId,
            TotalAmount = request.TotalAmount,
            AmountPaid = request.AmountPaid,
            RemainingAmount = request.TotalAmount - request.AmountPaid,
            Notes = request.Notes,
            CreatedByUserId = UserId,
            InvoiceDate = DateTime.Now
        };
        // Add Invoice
        if (!unitOfWork.InvoiceRepository.Add(invoice)) 
        {
            unitOfWork.Rollback();
            return Result.Failure<InvoiceResponse>(InvoiceErrors.InvoiceAddFailed);
        }

        // Map Invoice Items
        invoice.InvoiceItems = request.InvoiceItems.Select(itemRequest => new Domain.Entities.InvoiceItem
        {
            InvoiceId = invoice.InvoiceId,
            ProductId = itemRequest.ProductId,
            Qty = itemRequest.Qty,
            PricePerUnit = itemRequest.PricePerUnit,
            IsGift = itemRequest.IsGift
        }).ToList();
        // Add Invoice Items
        foreach (var item in invoice.InvoiceItems)
        {
            if (!unitOfWork.InvoiceItemRepository.Add(item)) 
            {
                unitOfWork.Rollback();
                return Result.Failure<InvoiceResponse>(InvoiceErrors.InvoiceAddFailed);
            }
        }


        //Update User Products
        foreach (var item in request.InvoiceItems)
        {
            var userProduct = User.UserProducts.FirstOrDefault(up => up.ProductId == item.ProductId);
            if (userProduct != null)
            {
                userProduct.Qty -= item.Qty;
                if (!unitOfWork.UserProductRepository.Update(userProduct))
                {
                    unitOfWork.Rollback();
                    return Result.Failure<InvoiceResponse>(InvoiceErrors.InvoiceAddFailed);
                }
            }
        }


        // If Amount Paid > 0, create Payment
        if (request.AmountPaid > 0) 
        {
            // Create Payment
            Domain.Entities.Payment payment = new Domain.Entities.Payment
            {
                Added = true,
                Amount = request.AmountPaid,
                CreatedByUserId = UserId,
                Finshed = false,
                InvoiceId = invoice.InvoiceId,
                Notes = request.Notes,
                PaymentDate = DateTime.Now,
                PaymentMethod = request.PaymentMethod
            };
            // Add Payment
            if (!unitOfWork.PaymentRepository.Add(payment))
            {
                unitOfWork.Rollback();
                return Result.Failure<InvoiceResponse>(InvoiceErrors.InvoiceAddFailed);
            }



            // Retrieve related data for response
            invoice.CreatedByUser = User;

            if (invoice.CustomerId != null)
                invoice.Customer = unitOfWork.CustomerRepository.Find((int)invoice.CustomerId);
        }



        // log 
        var realPrice = request.InvoiceItems.Sum(e => 
        {
            var product = unitOfWork.ProductRepository.Find(e.ProductId);
            return product.Price * e.Qty;
        });
        var giftsCount = request.InvoiceItems.Sum(e => e.IsGift? e.Qty : 0);
        byte level = 1;
        if (realPrice - request.TotalAmount > 40 || giftsCount > 4)
            level = 5;
        else if (realPrice - request.TotalAmount > 30 || giftsCount > 3)
            level = 4;
        else if (realPrice - request.TotalAmount > 20 || giftsCount > 2)
            level = 3;
        else if (realPrice - request.TotalAmount > 5 || giftsCount > 1)
            level = 2;

        if(level != 1)
        {
            var sRecord = new Domain.Entities.SystemRecord() 
            {
                CreatedDate = DateTime.Now,
                Finished = false,
                Level = level,
                Description = $"{User.Name} فاتوره رقم: {invoice.InvoiceId} يجب مراجعتها"
            };

            if (!unitOfWork.SystemRecordRepository.Add(sRecord)) 
            {
                unitOfWork.Rollback();
                return Result.Failure<InvoiceResponse>(InvoiceErrors.InvoiceAddFailed);
            }
        }





        var response = new InvoiceResponse
        {
            InvoiceId = invoice.InvoiceId,
            CustomerName = invoice.Customer?.CustomerName ?? "مجهول",
            InvoiceDate = invoice.InvoiceDate,
            TotalAmount = invoice.TotalAmount,
            AmountPaid = invoice.AmountPaid,
            RemainingAmount = invoice.RemainingAmount,
            CreatedBy = invoice.CreatedByUser?.Name ?? "مجهول",
            Notes = invoice.Notes
        };


        unitOfWork.Commit();
        return Result.Success(response);
    }

    public Result InvoicePayment(int UserId, AddInvoicePaymentRequest request)
    {
        
        var invoice = unitOfWork.InvoiceRepository.Find(request.InvoiceId);
        if (invoice is null)
            return Result.Failure(InvoiceErrors.InvoiceNotFound);

        Domain.Entities.Payment payment = new Domain.Entities.Payment
        {
            Added = true,
            Amount = request.AmountPaid,
            CreatedByUserId = UserId,
            Finshed = false,
            InvoiceId = request.InvoiceId,
            Notes = request.Notes,
            PaymentDate = DateTime.Now,
            PaymentMethod = request.PaymentMethod
        };

        // Add Payment
        if (!unitOfWork.PaymentRepository.Add(payment))
        {
            unitOfWork.Rollback();
            return Result.Failure(InvoiceErrors.InvoicePaymentAddFailed);
        }

        // Update Invoice Amounts
        invoice.AmountPaid += request.AmountPaid;
        invoice.RemainingAmount -= request.AmountPaid;
        if (!unitOfWork.InvoiceRepository.Update(invoice))
        {
            unitOfWork.Rollback();
            return Result.Failure(InvoiceErrors.InvoicePaymentAddFailed);
        }

        unitOfWork.Commit();
        return Result.Success();

    }

    /// <summary>
    /// Delete Invoice With Payments, This Effect On Reports, Be Carefull
    /// </summary>
    /// <param name="InvoiceId"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public Result FullDelete(int InvoiceId)
    {
        var invoice = unitOfWork.InvoiceRepository.Find(InvoiceId);

        var inPayments = unitOfWork.PaymentRepository.GetAll(InvoiceId);

        foreach (var item in inPayments)
            if(!unitOfWork.PaymentRepository.Delete(item.PaymentId))
                unitOfWork.Rollback();
        
        unitOfWork.InvoiceRepository.Delete(InvoiceId);
        unitOfWork.Commit();
        return Result.Success();
    }
}
