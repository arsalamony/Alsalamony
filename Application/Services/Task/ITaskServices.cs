

using Application.Common.Results;
using Application.Contracts.Task;

namespace Application.Services.Task;

public interface ITaskServices
{
    Result<TaskResponse> Get(int taskId);

    Result<IEnumerable<TaskResponse>> GetAll();

    Result<TaskResponse> Add(int userId, AddTaskRequest request);

    Result SetComplete(int taskId, int userId);

    Result SetCancel(int taskId, int userId, string Notes);

    Result SetNotes(int  taskId, int userId, string Notes);

    Result Delete(int taskId);
}
