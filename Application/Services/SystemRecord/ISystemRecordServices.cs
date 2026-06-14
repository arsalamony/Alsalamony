using Application.Common.Results;
using Application.Contracts.SystemRecord;
using System.Threading.Tasks;
using System.Collections.Generic;
using Alsalamony.Application.Common.Models;

namespace Application.Services.SystemRecord;

public interface ISystemRecordServices
{

    Task<Result<IEnumerable<SystemRecordsResponse>>> GetAllNotFinished(bool isAdmin);

    Task<Result<PaginatedList<SystemRecordsResponse>>> GetAllPaged(RequestFilters requestFilters, string userRole);
    Task<Result> Finish(int systemRecordId);
}
