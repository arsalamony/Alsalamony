
using Application.Contracts.Task;
using Task = Domain.Entities.Task;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories;

public interface ITaskRepository : IGenericRepository<Task>
{
    Task<IEnumerable<TaskResponse>> GetAll();

}
