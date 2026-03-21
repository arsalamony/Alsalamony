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

    public async Task<bool> Add(SystemRecord entity)
    {
        bool isAdded = false;

        using (SqlCommand command = ReposHelper.CreateCommand("AddSystemRecord", connection, transaction))
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

            int rowAffected = await command.ExecuteNonQueryAsync();

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

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<SystemRecord> Find(int id)
    {
        SystemRecord SystemRecord = null;

        string query = "Select * from SystemRecords Where SystemRecordId = @SystemRecordId;";

        using (SqlCommand command = ReposHelper.CreateCommand(query, connection, transaction))
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

    public async Task<IEnumerable<SystemRecord>> GetAll()
    {
        List<SystemRecord> rs = new List<SystemRecord>();
        using (SqlCommand command = ReposHelper.CreateCommand("Select * from SystemRecords Order by SystemRecordId desc;", connection, transaction))
        {
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
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

    public async Task<bool> Update(SystemRecord entity)
    {
        int rowAffected = 0;

        using (SqlCommand command = ReposHelper.CreateCommand("UpdateSystemRecord", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@SystemRecordId", entity.SystemRecordId);
            command.Parameters.AddWithValue("@Level", entity.Level);
            command.Parameters.AddWithValue("@Description", entity.Description);
            command.Parameters.AddWithValue("@Finished", entity.Finished);
            command.Parameters.AddWithValue("@CreatedDate", entity.CreatedDate);

            rowAffected = await command.ExecuteNonQueryAsync();
        }

        return rowAffected > 0;
    }
}
