using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.Payment;
using Domain.Enums;
using System.Linq;

namespace Application.Services.Payment;

public class PaymentServices : IPaymentServices
{
    private readonly IUnitOfWork unitOfWork;

    public PaymentServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }
    public Result<PaymentResponse> Get(int Id)
    {
        var payment = unitOfWork.PaymentRepository.Find(Id);

        if (payment == null)
            return Result.Failure<PaymentResponse>(PaymentErrors.PaymentNotFound);

        payment.CreatedByUser = unitOfWork.UserRepository.Find(payment.CreatedByUserId);

        var response = new PaymentResponse
        {
            PaymentId = payment.PaymentId,
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod == enPaymentMethod.Cash? "كاش": payment.PaymentMethod == enPaymentMethod.VodafoneCash? "تلفون المحل":"بابا",
            CreatedBy = payment.CreatedByUser.Name,
            Status = payment.Added ? "تحصيل" : "استقطاع",
            Finshed = payment.Finshed ? "منتهيه" : "غير منتهيه",
            Notes = payment.Notes
        };

        return Result.Success(response);
    }

    public Result<PaymentResponse> Add(int UserId, AddPaymentRequest request)
    {
        Domain.Entities.Payment payment = new Domain.Entities.Payment
        {
            Amount = request.Amount,
            PaymentDate = DateTime.Now,
            PaymentMethod = request.PaymentMethod,
            CreatedByUserId = UserId,
            Notes = request.Notes,
            Added = request.Added,
            Finshed = false
        };
        if (!unitOfWork.PaymentRepository.Add(payment))
        {
            unitOfWork.Rollback();
            return Result.Failure<PaymentResponse>(PaymentErrors.PaymentAddFailed);
        }



        payment.CreatedByUser = unitOfWork.UserRepository.Find(payment.CreatedByUserId);

        var response = new PaymentResponse
        {
            PaymentId = payment.PaymentId,
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod == enPaymentMethod.Cash ? "كاش" : payment.PaymentMethod == enPaymentMethod.VodafoneCash ? "تلفون المحل" : "بابا",
            CreatedBy = payment.CreatedByUser.Name,
            Status = payment.Added ? "تحصيل" : "استقطاع",
            Finshed = payment.Finshed ? "منتهيه" : "غير منتهيه",
            Notes = payment.Notes
        };

        unitOfWork.Commit();
        return Result.Success(response);
    }
    public Result<PaymentResponse> AddByAdmin(AddPaymentByAdminRequest request)
    {
        Domain.Entities.Payment payment = new Domain.Entities.Payment
        {
            Amount = request.Amount,
            PaymentDate = DateTime.Now,
            PaymentMethod = request.PaymentMethod,
            CreatedByUserId = request.CreatedByUserId,
            Notes = request.Notes,
            Added = request.Added,
            Finshed = false
        };

        if (!unitOfWork.PaymentRepository.Add(payment)) 
        {
            unitOfWork.Rollback();
            return Result.Failure<PaymentResponse>(PaymentErrors.PaymentAddFailed);
        }



        payment.CreatedByUser = unitOfWork.UserRepository.Find(payment.CreatedByUserId);

        var response = new PaymentResponse
        {
            PaymentId = payment.PaymentId,
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod == enPaymentMethod.Cash ? "كاش" : payment.PaymentMethod == enPaymentMethod.VodafoneCash ? "تلفون المحل" : "بابا",
            CreatedBy = payment.CreatedByUser.Name,
            Status = payment.Added ? "تحصيل" : "استقطاع",
            Finshed = payment.Finshed ? "منتهيه" : "غير منتهيه",
            Notes = payment.Notes
        };

        unitOfWork.Commit();
        return Result.Success(response);
    }

    // Get all payments
    public Result<IEnumerable<PaymentViewResponse>> GetAll()
    {
        var payments = unitOfWork.PaymentRepository.GetAll();
        foreach (var payment in payments)
        {
            payment.CreatedByUser = unitOfWork.UserRepository.Find(payment.CreatedByUserId);
        }
        var response = payments.OrderByDescending(e => e.PaymentDate).Select(payment => new PaymentViewResponse
        {
            PaymentId = payment.PaymentId,
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod == enPaymentMethod.Cash ? "كاش" : payment.PaymentMethod == enPaymentMethod.VodafoneCash ? "تلفون المحل" : "بابا",
            CreatedBy = payment.CreatedByUser!.Name,
            Added = payment.Added,
            Finshed = payment.Finshed,
            Notes = payment.Notes
        });
        return Result.Success(response);
    }


    /// <summary>
    /// Get all payments for a specific user (or all if Admin) that are not finished
    /// </summary>
    /// <param name="UserId"></param>
    /// <returns></returns>
    public Result<IEnumerable<PaymentViewResponse>> GetAll(int UserId, bool IsAdmin)
    {
        var payments = unitOfWork.PaymentRepository.GetAll();
        var paymentViews = payments.Where(e => (IsAdmin || e.CreatedByUserId == UserId) && !e.Finshed);

        foreach (var payment in paymentViews)
        {
            payment.CreatedByUser = unitOfWork.UserRepository.Find(payment.CreatedByUserId);
        }

        var response = paymentViews.OrderByDescending(e => e.PaymentDate).Select(payment => new PaymentViewResponse
        {
            PaymentId = payment.PaymentId,
            InvoiceId = payment.InvoiceId,
            Amount = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMethod = payment.PaymentMethod == enPaymentMethod.Cash ? "كاش" : payment.PaymentMethod == enPaymentMethod.VodafoneCash ? "تلفون المحل" : "بابا",
            CreatedBy = payment.CreatedByUser!.Name,
            Added = payment.Added,
            Finshed = payment.Finshed,
            Notes = payment.Notes
        });
        return Result.Success(response);
    }


    public Result FinshAllPayment(int UserId)
    {
        var payments = unitOfWork.PaymentRepository.GetAll();

        foreach (var payment in payments)
        {
            if(payment.CreatedByUserId == UserId && !payment.Finshed)
            {
                payment.Finshed = true;
                if (!unitOfWork.PaymentRepository.Update(payment))
                {
                    unitOfWork.Rollback();
                    return Result.Failure(PaymentErrors.PaymentUpdateFailed);
                }
            }
        }

        unitOfWork.Commit();
        return Result.Success();
    }


    public Result FinshPayment(int PaymentId)
    {
        var payment = unitOfWork.PaymentRepository.Find(PaymentId);

        if (payment == null)
            return Result.Failure(PaymentErrors.PaymentNotFound);

        payment.Finshed = true;

        if(!unitOfWork.PaymentRepository.Update(payment))
        {
            unitOfWork.Rollback();
            return Result.Failure(PaymentErrors.PaymentUpdateFailed);
        }

        unitOfWork.Commit();
        return Result.Success();
    }

}
