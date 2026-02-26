using Application.Common.Results;
using Application.Contracts.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Report;

public interface IReportServices
{

    Result<IEnumerable<DayIncome>> GetDayIncomeReport();
}
