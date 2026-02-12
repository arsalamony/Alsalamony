using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.Task;
using Domain.Entities;

namespace Application.Services.Task;

public class TaskServices : ITaskServices
{
    private readonly IUnitOfWork unitOfWork;

    public TaskServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }


    public Result<IEnumerable<TaskResponse>> GetAll()
    {
        var tasks = unitOfWork.TaskRepository.GetAll();

        return Result.Success(tasks);
    }

    public Result<TaskResponse> Get(int taskId)
    {

        var task = unitOfWork.TaskRepository.Find(taskId);

        if (task == null) return Result.Failure<TaskResponse>(TaskErrors.TaskNotFound);


        task.Address = unitOfWork.AddressRepository.Find(task.AddressId);
        task.CreatedByUser = unitOfWork.UserRepository.Find(task.CreatedByUserId);
        task.AssignedToUser = task.AssignedToUserId is null ? null : unitOfWork.UserRepository.Find((int)task.AssignedToUserId);
        task.CompletedByUser = task.CompletedByUserId is null ? null : unitOfWork.UserRepository.Find((int)task.CompletedByUserId);


        var res = new TaskResponse() 
        {
            Notes = task.Notes,
            Name = task.Name,
            TaskId = taskId,
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt,
            UpdatedAt = task.UpdatedAt,
            AddressName = task.Address.AddressName,
            Priority = task.Priority.ToString(),
            Status = task.Status.ToString(),
            AssignedTo = task.AssignedToUser?.Name,
            CreatedBy = task.CreatedByUser.Name,
            CompletedBy = task.CompletedByUser?.Name,
            AssignedToUserId = task.AssignedToUserId,
            CompletedByUserId = task.CompletedByUserId,
            CreatedByUserId = task.CreatedByUserId,
        };

        return Result.Success(res);
    }

    public Result<TaskResponse> Add(int userId, AddTaskRequest request)
    {
        var newTask = new Domain.Entities.Task() 
        {
            AddressId = request.AddressId,
            CreatedAt = DateTime.Now,
            Name = request.Name,
            AssignedToUserId = request.AssignedToUserId,
            CreatedByUserId = userId,
            Priority = request.Priority,
            Status = Domain.Enums.enTaskStatus.New,
            Notes = request.Notes,
            CompletedAt = null,
            CompletedByUserId = null,
            UpdatedAt = null,     
            
        };

        if (!unitOfWork.TaskRepository.Add(newTask)) 
        {
            unitOfWork.Rollback();
            return Result.Failure<TaskResponse>(TaskErrors.TaskAddFailed);
        }



        newTask.Address = unitOfWork.AddressRepository.Find(newTask.AddressId);
        newTask.CreatedByUser = unitOfWork.UserRepository.Find(newTask.CreatedByUserId);
        newTask.AssignedToUser = newTask.AssignedToUserId is null ? null : unitOfWork.UserRepository.Find((int)newTask.AssignedToUserId);
        newTask.CompletedByUser = newTask.CompletedByUserId is null ? null : unitOfWork.UserRepository.Find((int)newTask.CompletedByUserId);


        var res = new TaskResponse()
        {
            Notes = newTask.Notes,
            Name = newTask.Name,
            TaskId = newTask.TaskId,
            CreatedAt = newTask.CreatedAt,
            CompletedAt = newTask.CompletedAt,
            UpdatedAt = newTask.UpdatedAt,
            AddressName = newTask.Address.AddressName,
            Priority = newTask.Priority.ToString(),
            Status = newTask.Status.ToString(),
            AssignedTo = newTask.AssignedToUser?.Name,
            CreatedBy = newTask.CreatedByUser.Name,
            CompletedBy = newTask.CompletedByUser?.Name,
            AssignedToUserId = newTask.AssignedToUserId,
            CompletedByUserId = newTask.CompletedByUserId,
            CreatedByUserId = newTask.CreatedByUserId,
        };


        unitOfWork.Commit();
        return Result.Success(res);
    }

    public Result SetComplete(int taskId, int userId)
    {
        var t = unitOfWork.TaskRepository.Find(taskId);
        if (t is null)
            return Result.Failure(TaskErrors.TaskNotFound);


        t.Status = Domain.Enums.enTaskStatus.Done;
        t.CompletedByUserId = userId;
        t.CompletedAt = DateTime.Now;
        t.UpdatedAt = DateTime.Now;

        if (!unitOfWork.TaskRepository.Update(t)) 
        {
            unitOfWork.Rollback();
            return Result.Failure(TaskErrors.TaskAddFailed);
        }

        unitOfWork.Commit();
        return Result.Success();
    }

    public Result SetCancel(int taskId, int userId, string Notes)
    {
        var t = unitOfWork.TaskRepository.Find(taskId);
        if (t is null)
            return Result.Failure(TaskErrors.TaskNotFound);


        t.Status = Domain.Enums.enTaskStatus.Canceled;
        t.CompletedByUserId = userId;
        t.CompletedAt = DateTime.Now;
        t.UpdatedAt = DateTime.Now;
        t.Notes = Notes;

        if (!unitOfWork.TaskRepository.Update(t))
        {
            unitOfWork.Rollback();
            return Result.Failure(TaskErrors.TaskAddFailed);
        }

        unitOfWork.Commit();
        return Result.Success();
    }

    public Result SetNotes(int taskId, int userId, string Notes)
    {
        var t = unitOfWork.TaskRepository.Find(taskId);
        if (t is null)
            return Result.Failure(TaskErrors.TaskNotFound);


        t.Status = Domain.Enums.enTaskStatus.InProgress;
        t.UpdatedAt = DateTime.Now;
        t.Notes = Notes;

        if (!unitOfWork.TaskRepository.Update(t))
        {
            unitOfWork.Rollback();
            return Result.Failure(TaskErrors.TaskAddFailed);
        }

        unitOfWork.Commit();
        return Result.Success();
    }

    // write checks later
    public Result Delete(int taskId)
    {
        var r = unitOfWork.TaskRepository.Delete(taskId);

        unitOfWork.Commit();
        return Result.Success();
    }
}
