using Application.Common.Interfaces.Repositories;
using Application.Common.Results;
using Application.Contracts.Invoice;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics.Contracts;

namespace Infrastructure.Persistence.Repositories;

public class InvoiceRepository : IGenericRepository<Invoice>, IInvoiceRepository
{
    private SqlConnection connection;
    private SqlTransaction transaction;

    public InvoiceRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }
    public bool Add(Invoice entity)
    {
        bool isAdded = false;
        try
        {
            using (SqlCommand command = new SqlCommand("AddInvoice", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CustomerId", entity.CustomerId != null? entity.CustomerId:DBNull.Value);
                command.Parameters.AddWithValue("@InvoiceDate", entity.InvoiceDate);
                command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
                command.Parameters.AddWithValue("@AmountPaid", entity.AmountPaid);
                command.Parameters.AddWithValue("@CreatedByUserId", entity.CreatedByUserId);
                command.Parameters.AddWithValue("@Notes", entity.Notes != null? entity.Notes:DBNull.Value);


                command.Parameters.Add(new SqlParameter("@InvoiceId", SqlDbType.Int)
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
                    entity.InvoiceId = (int)command.Parameters["@InvoiceId"].Value;
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

    public Invoice Find(int id)
    {
        Invoice Invoice = null;

        try
        {
            using (SqlCommand command = new SqlCommand("GetInvoiceById", connection, transaction))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvoiceId", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // The record was found
                        Invoice = new Invoice
                        {
                            InvoiceId = id,
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : null,
                            InvoiceDate = (DateTime)reader["InvoiceDate"],
                            TotalAmount = (decimal)reader["TotalAmount"],
                            AmountPaid = (decimal)reader["AmountPaid"],
                            CreatedByUserId = (int)reader["CreatedByUserId"],
                            CustomerId = reader["CustomerId"] != DBNull.Value ? (int?)reader["CustomerId"] : null,
                            RemainingAmount = (decimal)reader["RemainingAmount"]
                        };

                    }

                }
            }

        }
        catch (Exception ex)
        {
            //Console.WriteLine("Error: " + ex.Message);

        }

        return Invoice;
    }

    public List<UnpaidInvoiceRow> GetAllUnpayedRowsByCustomerId(int customerId)
    {
        var rows = new List<UnpaidInvoiceRow>();

        try
        {
            using (SqlCommand command = new SqlCommand("GetAllUnpayedInvoicesByCustomerId", connection, transaction))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    int oProductName = reader.GetOrdinal("ProductName");
                    int oQty = reader.GetOrdinal("Qty");
                    int oPricePerUnit = reader.GetOrdinal("PricePerUnit");
                    int oIsGift = reader.GetOrdinal("IsGift");

                    while (reader.Read())
                    {

                        rows.Add(new UnpaidInvoiceRow
                        {
                            InvoiceId = (int)reader["InvoiceId"],
                            CustomerName = (string)reader["CustomerName"],
                            InvoiceDate = (DateTime)reader["InvoiceDate"],
                            TotalAmount = (decimal)reader["TotalAmount"],
                            AmountPaid = (decimal)reader["AmountPaid"],
                            RemainingAmount = (decimal)reader["RemainingAmount"],
                            CreatedBy = (string)reader["CreatedBy"],
                            Notes = reader["Notes"] == DBNull.Value ? null : (string)reader["Notes"],

                            ProductName = reader["ProductName"] == DBNull.Value ? null : (string)reader["ProductName"],
                            Qty = reader["Qty"] == DBNull.Value ? null : (int)reader["Qty"],
                            PricePerUnit = reader["PricePerUnit"] == DBNull.Value ? null : (decimal)reader["PricePerUnit"],
                            IsGift = reader["IsGift"] == DBNull.Value ? null : (bool)reader["IsGift"],
                        });

                    }
                }
            }

        }
        catch (Exception)
        {

            throw;
        }

        return rows; // هنا الاتصال اتقفل خلاص
    }


    /// <summary>
    /// return all invoice order by Invoice Date Desc
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    public ICollection<Invoice> GetInvoicesByCustomerId(int customerId)
    {
        List<Invoice> Invoices = new List<Invoice>();

        try
        {
            using (SqlCommand command = new SqlCommand("GetInvoicesByCustomerId", connection, transaction))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerId", customerId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Invoices.Add(new Invoice
                        {
                            CustomerId = customerId,
                            InvoiceId = (int)reader["InvoiceId"],
                            Notes = reader["Notes"] != DBNull.Value ? (string)reader["Notes"] : null,
                            InvoiceDate = (DateTime)reader["InvoiceDate"],
                            TotalAmount = (decimal)reader["TotalAmount"],
                            AmountPaid = (decimal)reader["AmountPaid"],
                            CreatedByUserId = (int)reader["CreatedByUserId"],
                            RemainingAmount = (decimal)reader["RemainingAmount"]
                        });
                    }
                }
            }

        }
        catch (Exception)
        {

            throw;
        }

        return Invoices;
    }

    public bool Update(Invoice entity)
    {
        int rowAffected = 0;
        try
        {
            using (SqlCommand command = new SqlCommand("UpdateInvoice", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@InvoiceId", entity.InvoiceId);
                command.Parameters.AddWithValue("@CustomerId", entity.CustomerId != null ? entity.CustomerId : DBNull.Value);
                command.Parameters.AddWithValue("@InvoiceDate", entity.InvoiceDate);
                command.Parameters.AddWithValue("@TotalAmount", entity.TotalAmount);
                command.Parameters.AddWithValue("@AmountPaid", entity.AmountPaid);
                command.Parameters.AddWithValue("@CreatedByUserId", entity.CreatedByUserId);
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
