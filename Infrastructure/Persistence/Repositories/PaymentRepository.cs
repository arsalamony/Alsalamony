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

    public bool Add(Payment entity)
    {
        bool isAdded = false;
        try
        {
            using (SqlCommand command = new SqlCommand("AddPayment", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@InvoiceId", entity.InvoiceId != null? entity.InvoiceId:DBNull.Value);
                command.Parameters.AddWithValue("@Amount", entity.Amount);
                command.Parameters.AddWithValue("@PaymentDate", entity.PaymentDate);
                command.Parameters.AddWithValue("@PaymentMethod", (byte)entity.PaymentMethod);
                command.Parameters.AddWithValue("@CreatedByUserId", entity.CreatedByUserId);
                command.Parameters.AddWithValue("@Added", entity.Added);
                command.Parameters.AddWithValue("@Finshed", entity.Finshed);
                command.Parameters.AddWithValue("@Notes", entity.Notes != null? entity.Notes:DBNull.Value);

                command.Parameters.Add(new SqlParameter("@PaymentId", SqlDbType.Int)
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
                    entity.PaymentId = (int)command.Parameters["@PaymentId"].Value;
                }
            }

        }
        catch (Exception)
        {

            throw;
        }

        return isAdded;
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Payment Find(int id)
    {
        Payment Payment = null;

        try
        {

            using (SqlCommand command = new SqlCommand("GetPaymentById", connection, transaction))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@PaymentId", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
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

        }
        catch (Exception ex)
        {
            //Console.WriteLine("Error: " + ex.Message);

        }


        return Payment;
    }

    public IEnumerable<Payment> GetAll()
    {
        List<Payment> Payments = new List<Payment>();
        try
        {
            using (SqlCommand command = new SqlCommand("GetAllPayments", connection, transaction))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
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

        }
        catch (Exception)
        {

            throw;
        }
        return Payments;
    }

    public bool Update(Payment entity)
    {
        int rowAffected = 0;
        try
        {
            using (SqlCommand command = new SqlCommand("UpdatePayment", connection, transaction))
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

                rowAffected = command.ExecuteNonQuery();
            }

        }
        catch (Exception)
        {

            throw;
        }

        return rowAffected > 0;
    }


}
