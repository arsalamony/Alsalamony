

using Domain.Enums;

namespace Application.Contracts.ProductTransaction;

public class AddProductTransactionRequest
{
    public int ProductId { get; set; }

    public decimal Amount { get; set; } = 0;

    public string? PersonName { get; set; }

    public enTransactionType TransactionType { get; set; }
    public int Qty { get; set; }
    public int CreatedByUserId { get; set; }
    public string? Notes { get; set; }
}
