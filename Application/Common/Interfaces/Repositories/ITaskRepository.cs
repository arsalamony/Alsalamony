
using Application.Contracts.Task;
using Task = Domain.Entities.Task;


namespace Application.Common.Interfaces.Repositories;

public interface ITaskRepository : IGenericRepository<Task>
{
    IEnumerable<TaskResponse> GetAll();

}
