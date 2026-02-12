using Application.Common.Interfaces.Repositories;
using Application.Contracts.Task;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Data.SqlClient;
using System.Data;
using Task = Domain.Entities.Task;
namespace Infrastructure.Persistence.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly SqlConnection connection;
    private readonly SqlTransaction transaction;

    public TaskRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public IEnumerable<TaskResponse> GetAll()
    {
        List<TaskResponse> tasks = new List<TaskResponse>();

        try
        {
            using (SqlCommand command = new SqlCommand("SP_GetAllTasks", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        tasks.Add(new TaskResponse
                        {
                            TaskId = (int)reader["TaskId"],
                            Name = (string)reader["Name"],
                            AddressName = (string)reader["AddressName"],
                            CreatedAt = (DateTime)reader["CreatedAt"],
                            Priority = (string)reader["Priority"],
                            Status = (string)reader["Status"],
                            CreatedByUserId = (int)reader["CreatedByUserId"],
                            CreatedBy = (string)reader["CreatedBy"],
                            AssignedTo = reader["AssignedTo"] != DBNull.Value? (string)reader["AssignedTo"] :null,
                            AssignedToUserId = reader["AssignedToUserId"] != DBNull.Value ? (int)reader["AssignedToUserId"] : null,
                            CompletedBy = reader["CompletedBy"] != DBNull.Value ? (string)reader["CompletedBy"] : null,
                            CompletedByUserId = reader["CompletedByUserId"] != DBNull.Value ? (int)reader["CompletedByUserId"] : null,
                            CompletedAt = reader["CompletedAt"] != DBNull.Value ? (DateTime)reader["CompletedAt"] : null,
                            UpdatedAt = reader["UpdatedAt"] != DBNull.Value ? (DateTime)reader["UpdatedAt"] : null,
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : null
                        });
                    }
                }
            }
        }
        catch
        {
            throw;
        }

        return tasks;
    }

    public Task Find(int id)
    {
        Task? task = null;

        try
        {
            using (SqlCommand command = new SqlCommand("SP_GetTaskById", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TaskId", id);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int ordTaskId = reader.GetOrdinal("TaskId");
                        int ordName = reader.GetOrdinal("Name");
                        int ordPriority = reader.GetOrdinal("Priority");
                        int ordStatus = reader.GetOrdinal("Status");
                        int ordAddressId = reader.GetOrdinal("AddressId");
                        int ordAssignedToUserId = reader.GetOrdinal("AssignedToUserId");
                        int ordCreatedAt = reader.GetOrdinal("CreatedAt");
                        int ordUpdatedAt = reader.GetOrdinal("UpdatedAt");
                        int ordCompletedAt = reader.GetOrdinal("CompletedAt");
                        int ordCreatedByUserId = reader.GetOrdinal("CreatedByUserId");
                        int ordCompletedByUserId = reader.GetOrdinal("CompletedByUserId");
                        int ordNotes = reader.GetOrdinal("Notes");

                        task = new Task
                        {
                            TaskId = reader.GetInt32(ordTaskId),
                            Name = reader.GetString(ordName),

                            Priority = (enTaskPriority)reader.GetByte(ordPriority),
                            Status = (enTaskStatus)reader.GetByte(ordStatus),

                            AddressId = reader.GetInt32(ordAddressId),

                            AssignedToUserId = reader.IsDBNull(ordAssignedToUserId)
                                ? (int?)null
                                : reader.GetInt32(ordAssignedToUserId),

                            CreatedAt = reader.GetDateTime(ordCreatedAt),

                            UpdatedAt = reader.IsDBNull(ordUpdatedAt)
                                ? (DateTime?)null
                                : reader.GetDateTime(ordUpdatedAt),

                            CompletedAt = reader.IsDBNull(ordCompletedAt)
                                ? (DateTime?)null
                                : reader.GetDateTime(ordCompletedAt),

                            CreatedByUserId = reader.GetInt32(ordCreatedByUserId),

                            CompletedByUserId = reader.IsDBNull(ordCompletedByUserId)
                                ? (int?)null
                                : reader.GetInt32(ordCompletedByUserId),

                            Notes = reader.IsDBNull(ordNotes)
                                ? null
                                : reader.GetString(ordNotes),
                        };
                    }
                }
            }
        }
        catch
        {
            throw;
        }

        return task;
    }

    public bool Add(Task entity)
    {
        bool isAdded = false;

        try
        {
            using (SqlCommand command = new SqlCommand("SP_AddTask", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Priority", (byte)entity.Priority);
                command.Parameters.AddWithValue("@Status", (byte)entity.Status);
                command.Parameters.AddWithValue("@AddressId", entity.AddressId);

                command.Parameters.AddWithValue("@AssignedToUserId",
                    (object?)entity.AssignedToUserId ?? DBNull.Value);

                // لو انت بتبعت CreatedAt من الكود
                command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);

                command.Parameters.AddWithValue("@CreatedByUserId", entity.CreatedByUserId);

                command.Parameters.AddWithValue("@Notes",
                    (object?)entity.Notes ?? DBNull.Value);

                command.Parameters.Add(new SqlParameter("@TaskId", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                });

                command.ExecuteNonQuery();

                entity.TaskId = (int)command.Parameters["@TaskId"].Value;
                isAdded = entity.TaskId > 0;
            }
        }
        catch
        {
            throw;
        }

        return isAdded;
    }

    public bool Update(Task entity)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SP_UpdateTask", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@TaskId", entity.TaskId);
                command.Parameters.AddWithValue("@Name", entity.Name);
                command.Parameters.AddWithValue("@Priority", (byte)entity.Priority);
                command.Parameters.AddWithValue("@Status", (byte)entity.Status);
                command.Parameters.AddWithValue("@AddressId", entity.AddressId);
                command.Parameters.AddWithValue("@CreatedByUserId", entity.CreatedByUserId);
                command.Parameters.AddWithValue("@CreatedAt", entity.CreatedAt);


                command.Parameters.AddWithValue("@AssignedToUserId",
                    (object?)entity.AssignedToUserId ?? DBNull.Value);

                command.Parameters.AddWithValue("@UpdatedAt",
                    (object?)entity.UpdatedAt ?? DBNull.Value);

                command.Parameters.AddWithValue("@CompletedAt",
                    (object?)entity.CompletedAt ?? DBNull.Value);

                command.Parameters.AddWithValue("@CompletedByUserId",
                    (object?)entity.CompletedByUserId ?? DBNull.Value);

                command.Parameters.AddWithValue("@Notes",
                    (object?)entity.Notes ?? DBNull.Value);

                // لو SP_UpdateTask بيرجع SELECT RowsAffected
                int rows = command.ExecuteNonQuery();

                return rows > 0;
            }
        }
        catch
        {
            throw;
        }
    }

    public bool Delete(int id)
    {
        try
        {
            using (SqlCommand command = new SqlCommand("SP_DeleteTask", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TaskId", id);

                // لو SP_DeleteTask بيرجع SELECT RowsAffected
                object? result = command.ExecuteScalar();
                int rows = (result == null || result == DBNull.Value) ? 0 : Convert.ToInt32(result);

                return rows > 0;
            }
        }
        catch
        {
            throw;
        }
    }
}