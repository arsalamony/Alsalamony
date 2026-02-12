using Domain.Entities;

namespace Application.Common.Interfaces.Repositories;

public interface ISystemRecordRepository : IGenericRepository<SystemRecord>
{
    IEnumerable<SystemRecord> GetAll();
}
