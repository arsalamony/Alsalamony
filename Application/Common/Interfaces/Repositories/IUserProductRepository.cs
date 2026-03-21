using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories;

public interface IUserProductRepository : IGenericRepository<UserProduct>
{
    Task<ICollection<UserProduct>> GetAll(int UserId);
}
