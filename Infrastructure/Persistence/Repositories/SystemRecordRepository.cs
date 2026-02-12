using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Infrastructure.Persistence.Repositories;

public class SystemRecordRepository : ISystemRecordRepository
{
    private SqlConnection connection;
    private SqlTransaction transaction;

    public SystemRecordRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public bool Add(SystemRecord entity)
    {
        bool isAdded = false;

        using (SqlCommand command = new SqlCommand("AddSystemRecord", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Description", entity.Description);
            command.Parameters.AddWithValue("@Level", entity.Level);
            command.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);
            command.Parameters.AddWithValue("@Finished", entity.Finished);

            command.Parameters.Add(new SqlParameter("@SystemRecordId", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output,
            });

            int rowAffected = command.ExecuteNonQuery();

            if (rowAffected == 0)  // this means insert failed
            {
                isAdded = false;
            }
            else
            {
                isAdded = true;
                entity.SystemRecordId = (int)command.Parameters["@SystemRecordId"].Value;
            }
        }


        return isAdded;
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public SystemRecord Find(int id)
    {
        SystemRecord SystemRecord = null;

        string query = "Select * from SystemRecords Where SystemRecordId = @SystemRecordId;";

        using (SqlCommand command = new SqlCommand(query, connection, transaction))
        {
            command.Parameters.AddWithValue("@SystemRecordId", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    // The record was found
                    SystemRecord = new SystemRecord
                    {
                        SystemRecordId = id,
                        CreatedDate = (DateTime)reader["CreatedDate"],
                        Description = (string)reader["Description"],
                        Finished = (bool)reader["Finished"],
                        Level = (byte)reader["Level"]
                    };

                }

            }
        }

        return SystemRecord;
    }

    public IEnumerable<SystemRecord> GetAll()
    {
        List<SystemRecord> rs = new List<SystemRecord>();

        using (SqlCommand command = new SqlCommand("Select * from SystemRecords Order by SystemRecordId desc;", connection, transaction))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    rs.Add(new SystemRecord
                    {
                        SystemRecordId = reader.GetInt32(reader.GetOrdinal("SystemRecordId")),
                        CreatedDate = (DateTime)reader["CreatedDate"],
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        Level = (byte)reader["Level"],
                        Finished = (bool)reader["Finished"]
                    });
                }
            }
        }

        return rs;
    }

    public bool Update(SystemRecord entity)
    {
        int rowAffected = 0;

        using (SqlCommand command = new SqlCommand("UpdateSystemRecord", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SystemRecordId", entity.SystemRecordId);
            command.Parameters.AddWithValue("@Level", entity.Level);
            command.Parameters.AddWithValue("@Description", entity.Description);
            command.Parameters.AddWithValue("@Finished", entity.Finished);
            command.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);

            rowAffected = command.ExecuteNonQuery();
        }

        return rowAffected > 0;
    }
}
