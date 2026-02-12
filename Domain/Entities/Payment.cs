namespace Domain.Entities;

using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;


public class Payment
{
    public int PaymentId { get; set; }
    public int? InvoiceId { get; set; } = null;
    public decimal Amount { get; set; } = 0;
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public enPaymentMethod PaymentMethod { get; set; } = enPaymentMethod.Cash;
    public int CreatedByUserId { get; set; }

    public User? CreatedByUser { get; set; } = null;

    public bool Added { get; set; } = true;

    public bool Finshed { get; set; } = false;

    public string? Notes { get; set; } = null;
}
