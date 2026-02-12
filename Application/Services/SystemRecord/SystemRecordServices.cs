using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.SystemRecord;


namespace Application.Services.SystemRecord;

public class SystemRecordServices : ISystemRecordServices
{
    private readonly IUnitOfWork unitOfWork;

    public SystemRecordServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Result Finish(int systemRecordId)
    {
        var sr = unitOfWork.SystemRecordRepository.Find(systemRecordId);

        if(sr is null)
            return Result.Failure(SystemRecordErrors.SystemRecordNotFound);

        sr.Finished = true;

        if (!unitOfWork.SystemRecordRepository.Update(sr))
            return Result.Failure(SystemRecordErrors.SystemRecordUpdateFailed);

        return Result.Success();
    }

    public Result<IEnumerable<SystemRecordsResponse>> GetAllNotFinished(bool isAdmin)
    {
        var sRs = unitOfWork.SystemRecordRepository.GetAll().ToList();

        List<SystemRecordsResponse> r = new List<SystemRecordsResponse>();
        if(isAdmin)
        {
            r = sRs.Where(r => !r.Finished).Select(r => new SystemRecordsResponse() 
            {
                Finished = r.Finished,
                CreatedDate = r.CreatedDate,
                Description = r.Description,
                Level = r.Level,
                SystemRecordId = r.SystemRecordId
            }).ToList();
        }
        else 
        {
            r = sRs.Where(r => !r.Finished && r.Level == 1).Select(r => new SystemRecordsResponse()
            {
                Finished = r.Finished,
                CreatedDate = r.CreatedDate,
                Description = r.Description,
                Level = r.Level,
                SystemRecordId = r.SystemRecordId
            }).ToList();
        }

        return Result.Success<IEnumerable<SystemRecordsResponse>>(r);
    }
}
