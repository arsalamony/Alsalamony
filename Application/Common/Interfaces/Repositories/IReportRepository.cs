using Application.Common.Results;
using Application.Contracts.Report;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Repositories;

public interface IReportRepository
{
    Task<IEnumerable<DayIncome>> GetDailyIncomeReport();
}
