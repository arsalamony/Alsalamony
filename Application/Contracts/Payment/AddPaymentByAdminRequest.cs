using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Contracts.Payment;

public class AddPaymentByAdminRequest
{
    public decimal Amount { get; set; }

    public enPaymentMethod PaymentMethod { get; set; }

    public int CreatedByUserId { get; set; }

    public bool Added {  get; set; }

    public string? Notes { get; set; }
}
