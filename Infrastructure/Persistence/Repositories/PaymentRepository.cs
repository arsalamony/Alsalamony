using Application.Common.Interfaces.Repositories;
using Application.Contracts.Payment;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;


namespace Infrastructure.Persistence.Repositories;

public class PaymentRepository : IGenericRepository<Payment>, IPaymentRepository
{
    private SqlConnection connection;
    private SqlTransaction transaction;

    public PaymentRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public async Task<bool> Add(Payment entity)
    {
        bool isAdded = false;

        using (SqlCommand command = ReposHelper.CreateCommand("AddPayment", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@InvoiceId", entity.InvoiceId != null ? entity.InvoiceId : DBNull.Value);
            command.Parameters.AddWithValue("@Amount", entity.Amount);
            command.Parameters.AddWithValue("@PaymentDate", entity.PaymentDate);
            command.Parameters.AddWithValue("@PaymentMethod", (byte)entity.PaymentMethod);
            command.Parameters.AddWithValue("@CreatedByUserId", entity.CreatedByUserId);
            command.Parameters.AddWithValue("@Added", entity.Added);
            command.Parameters.AddWithValue("@Finshed", entity.Finshed);
            command.Parameters.AddWithValue("@Notes", entity.Notes != null ? entity.Notes : DBNull.Value);

            command.Parameters.Add(new SqlParameter("@PaymentId", SqlDbType.Int)
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
                entity.PaymentId = (int)command.Parameters["@PaymentId"].Value;
            }
        }

        return isAdded;
    }

    public async Task<bool> Delete(int id)
    {
        int rowAffected = 0;

        using (SqlCommand command = ReposHelper.CreateCommand("Delete from Payments where PaymentId = @PaymentId;", connection, transaction))
        {
            command.Parameters.AddWithValue("@PaymentId", id);

            rowAffected = await command.ExecuteNonQueryAsync();
        }

        return rowAffected > 0;
    }

    public async Task<Payment> Find(int id)
    {
        Payment Payment = null;

        using (SqlCommand command = ReposHelper.CreateCommand("GetPaymentById", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PaymentId", id);
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    // The record was found
                    Payment = new Payment();

                    Payment.PaymentId = id;
                    Payment.InvoiceId = reader.IsDBNull(reader.GetOrdinal("InvoiceId")) ? null : reader.GetInt32(reader.GetOrdinal("InvoiceId"));
                    Payment.Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));
                    Payment.PaymentDate = (DateTime)reader["PaymentDate"];
                    Payment.PaymentMethod = (Domain.Enums.enPaymentMethod)(byte)reader["PaymentMethod"];
                    Payment.CreatedByUserId = reader.GetInt32(reader.GetOrdinal("CreatedByUserId"));
                    Payment.Added = reader.GetBoolean(reader.GetOrdinal("Added"));
                    Payment.Finshed = reader.GetBoolean(reader.GetOrdinal("Finshed"));
                    Payment.Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"));


                }

            }
        }

        return Payment;
    }

    public async Task<IEnumerable<Payment>> GetAll()
    {
        List<Payment> Payments = new List<Payment>();

        using (SqlCommand command = ReposHelper.CreateCommand("GetAllPayments", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {

                    Payments.Add(new Payment
                    {
                        PaymentId = (int)reader["PaymentId"],
                        InvoiceId = reader.IsDBNull(reader.GetOrdinal("InvoiceId")) ? null : reader.GetInt32(reader.GetOrdinal("InvoiceId")),
                        Amount = (decimal)reader["Amount"],
                        PaymentDate = (DateTime)reader["PaymentDate"],
                        PaymentMethod = (Domain.Enums.enPaymentMethod)(byte)reader["PaymentMethod"],
                        CreatedByUserId = (int)reader["CreatedByUserId"],
                        Added = (bool)reader["Added"],
                        Finshed = (bool)reader["Finshed"],
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
                    });
                }
            }
        }

        return Payments;
    }

    public async Task<IEnumerable<Payment>> GetAll(int InvoiceId)
    {
        List<Payment> Payments = new List<Payment>();

        using (SqlCommand command = ReposHelper.CreateCommand("GetAllPaymentsByInvoiceId", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@InvoiceId", InvoiceId);


            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {

                    Payments.Add(new Payment
                    {
                        PaymentId = (int)reader["PaymentId"],
                        InvoiceId = InvoiceId,
                        Amount = (decimal)reader["Amount"],
                        PaymentDate = (DateTime)reader["PaymentDate"],
                        PaymentMethod = (Domain.Enums.enPaymentMethod)(byte)reader["PaymentMethod"],
                        CreatedByUserId = (int)reader["CreatedByUserId"],
                        Added = (bool)reader["Added"],
                        Finshed = (bool)reader["Finshed"],
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
                    });
                }
            }
        }

        return Payments;
    }

    public async Task<IEnumerable<PaymentViewResponse>> GetAllPaged(int PageNo, int RowsNo)
    {
        List<PaymentViewResponse> Payments = new List<PaymentViewResponse>();

        using (SqlCommand command = ReposHelper.CreateCommand("SP_GetPaymentsPaged", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@PageNumber", PageNo);
            command.Parameters.AddWithValue("@PageSize", RowsNo);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {

                    Payments.Add(new PaymentViewResponse
                    {
                        PaymentId = (int)reader["PaymentId"],
                        InvoiceId = reader.IsDBNull(reader.GetOrdinal("InvoiceId")) ? null : reader.GetInt32(reader.GetOrdinal("InvoiceId")),
                        Amount = (decimal)reader["Amount"],
                        PaymentDate = (DateTime)reader["PaymentDate"],
                        PaymentMethod = ((byte)reader["PaymentMethod"]) == 1? "كاش": ((byte)reader["PaymentMethod"]) == 2 ? "تلفون المحل":"بابا",
                        CreatedBy = (string)reader["Name"],
                        Added = (bool)reader["Added"],
                        Finshed = (bool)reader["Finshed"],
                        Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
                    });
                }
            }
        }

        return Payments;
    }

    public async Task<int> GetPaymentNo()
    {
        int PaymentNo = 0;

            using (SqlCommand command = ReposHelper.CreateCommand("SELECT COUNT(*) FROM Payments;", connection, transaction))
            {
                var result = await command.ExecuteScalarAsync();
                PaymentNo = (result == null || result == DBNull.Value) ? 0 : Convert.ToInt32(result);
        }
        return PaymentNo;
    }

    public async Task<bool> Update(Payment entity)
    {
        int rowAffected = 0;

        using (SqlCommand command = ReposHelper.CreateCommand("UpdatePayment", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@PaymentId", entity.PaymentId);
            command.Parameters.AddWithValue("@InvoiceId", entity.InvoiceId != null ? entity.InvoiceId : DBNull.Value);
            command.Parameters.AddWithValue("@Amount", entity.Amount);
            command.Parameters.AddWithValue("@PaymentDate", entity.PaymentDate);
            command.Parameters.AddWithValue("@PaymentMethod", (byte)entity.PaymentMethod);
            command.Parameters.AddWithValue("@CreatedByUserId", entity.CreatedByUserId);
            command.Parameters.AddWithValue("@Added", entity.Added);
            command.Parameters.AddWithValue("@Finshed", entity.Finshed);
            command.Parameters.AddWithValue("@Notes", entity.Notes != null ? entity.Notes : DBNull.Value);

            rowAffected = await command.ExecuteNonQueryAsync();
        }

        return rowAffected > 0;
    }


}
