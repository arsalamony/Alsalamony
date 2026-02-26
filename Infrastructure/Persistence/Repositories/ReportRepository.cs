using Application.Common.Interfaces.Repositories;
using Application.Contracts.Report;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure.Persistence.Repositories;

public class ReportRepository : IReportRepository
{
    private SqlConnection connection;
    private SqlTransaction transaction;

    public ReportRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public IEnumerable<DayIncome> GetDailyIncomeReport()
    {
        List<DayIncome> DayIncomes = new List<DayIncome>();

        using (SqlCommand command = new SqlCommand("SP_MonthlyIncomeReport", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // The record was found
                    DayIncomes.Add(new DayIncome
                    {
                        Date = (DateTime)reader["Day"],
                        TotalIncome = (decimal)reader["TotalAddedPayments"]
                    });

                }

            }
        }

        return DayIncomes;
    }

}
