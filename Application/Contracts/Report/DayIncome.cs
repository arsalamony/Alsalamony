using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Report;

public class DayIncome
{
    public DateTime Date { get; set; }
    public decimal TotalIncome { get; set; }
}
