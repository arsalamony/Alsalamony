using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.Report;


namespace Application.Services.Report;

public class ReportServices : IReportServices
{
    private readonly IUnitOfWork unitOfWork;

    public ReportServices(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public Result<IEnumerable<DayIncome>> GetDayIncomeReport()
    {
        var re = unitOfWork.ReportRepository.GetDailyIncomeReport();

        return Result.Success(re);
    }
}
