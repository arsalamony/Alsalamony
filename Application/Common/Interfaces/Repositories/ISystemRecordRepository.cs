using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories;

public interface ISystemRecordRepository : IGenericRepository<SystemRecord>
{
    Task<IEnumerable<SystemRecord>> GetAll();
}
