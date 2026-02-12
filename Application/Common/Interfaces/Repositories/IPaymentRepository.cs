using Application.Contracts.Payment;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories;

public interface IPaymentRepository : IGenericRepository<Payment>
{
    IEnumerable<Payment> GetAll();

}
