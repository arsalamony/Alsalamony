using Application.Common.Results;
using Application.Contracts.Report;


namespace Application.Common.Interfaces.Repositories;

public interface IReportRepository
{
    IEnumerable<DayIncome> GetDailyIncomeReport();
}
