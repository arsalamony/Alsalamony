using Application.Common.Results;
using Application.Contracts.SystemRecord;


namespace Application.Services.SystemRecord;

public interface ISystemRecordServices
{

    Result<IEnumerable<SystemRecordsResponse>> GetAllNotFinished(bool isAdmin);

    Result Finish(int systemRecordId);
}
