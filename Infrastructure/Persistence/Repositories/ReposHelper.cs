using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories;

internal static class ReposHelper
{
    public static SqlCommand CreateCommand(string query, SqlConnection connection, SqlTransaction transaction)
    {
        if (connection == null || String.IsNullOrEmpty(query))
            throw new ArgumentNullException();

        if (transaction == null) 
        {
            SqlCommand cmd = new SqlCommand(query, connection);
            return cmd;
        }

        SqlCommand command = new SqlCommand(query, connection, transaction);
        return command;
    }
}
