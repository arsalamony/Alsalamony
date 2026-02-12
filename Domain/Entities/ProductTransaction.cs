namespace Domain.Entities;
using Domain.Enums;
using System;



public class ProductTransaction
{
    public int ProductTransactionId { get; set; }

    public int ProductId { get; set; }

    public int? PaymentId { get; set; }

    public string? PersonName { get; set; }

    public enTransactionType TransactionType { get; set; }

    public int Qty { get; set; }

    public DateTime Date { get; set; }

    public int CreatedByUserId { get; set; }

    public string? Notes { get; set; }
}
