using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories;

public interface IUserProductRepository : IGenericRepository<UserProduct>
{
    ICollection<UserProduct> GetAll(int UserId);
}
