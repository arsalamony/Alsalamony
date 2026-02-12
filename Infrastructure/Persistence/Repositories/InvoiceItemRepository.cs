using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Persistence.Repositories;

public class InvoiceItemRepository : IGenericRepository<InvoiceItem>, IInvoiceItemRepository
{
    private SqlConnection connection;
    private SqlTransaction transaction;

    public InvoiceItemRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }
    public bool Add(InvoiceItem entity)
    {
        bool isAdded = false;
        try
        {
            using (SqlCommand command = new SqlCommand("SP_AddInvoiceItem", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@InvoiceId", entity.InvoiceId);
                command.Parameters.AddWithValue("@ProductId", entity.ProductId);
                command.Parameters.AddWithValue("@Qty", entity.Qty);
                command.Parameters.AddWithValue("@PricePerUnit", entity.PricePerUnit);
                command.Parameters.AddWithValue("@IsGift", entity.IsGift);

                command.Parameters.Add(new SqlParameter("@InvoiceItemId", SqlDbType.Int)
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
                    entity.InvoiceItemId = (int)command.Parameters["@InvoiceItemId"].Value;
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

    public InvoiceItem Find(int id)
    {
        throw new NotImplementedException();
        //InvoiceItem InvoiceItem = null;

        //try
        //{

        //    using (SqlCommand command = new SqlCommand("GetInvoiceItemById", connection, transaction))
        //    {
        //        command.CommandType = System.Data.CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@InvoiceItemId", id);
        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            if (reader.Read())
        //            {
        //                // The record was found
        //                InvoiceItem = new InvoiceItem
        //                {
        //                    InvoiceItemId = id,
        //                    InvoiceId = reader.IsDBNull(reader.GetOrdinal("InvoiceId")) ? null : reader.GetInt32(reader.GetOrdinal("InvoiceId")),
        //                    Amount = reader.GetDecimal(reader.GetOrdinal("Amount")),
        //                    InvoiceItemDate = reader.GetDateTime(reader.GetOrdinal("InvoiceItemDate")),
        //                    InvoiceItemMethod = (Domain.Enums.enInvoiceItemMethod)reader.GetInt32(reader.GetOrdinal("InvoiceItemMethod")),
        //                    CreatedByUserId = reader.GetInt32(reader.GetOrdinal("CreatedByUserId")),
        //                    Added = reader.GetBoolean(reader.GetOrdinal("Added")),
        //                    Finshed = reader.GetBoolean(reader.GetOrdinal("Finshed")),
        //                    Notes = reader.IsDBNull(reader.GetOrdinal("Notes")) ? null : reader.GetString(reader.GetOrdinal("Notes"))
        //                };

        //            }

        //        }
        //    }

        //}
        //catch (Exception ex)
        //{
        //    //Console.WriteLine("Error: " + ex.Message);

        //}


        //return InvoiceItem;
    }

    public ICollection<InvoiceItem> GetInvoiceItems(int InvoiceId)
    {
        List<InvoiceItem> InvoiceItems = new List<InvoiceItem>();
        try
        {
            using (SqlCommand command = new SqlCommand("SP_GetAllInvoiceItems", connection, transaction))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@InvoiceId", InvoiceId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        InvoiceItems.Add(new InvoiceItem
                        {
                            InvoiceItemId = reader.GetInt32(reader.GetOrdinal("InvoiceItemId")),
                            InvoiceId = InvoiceId,
                            ProductId = reader.GetInt32(reader.GetOrdinal("ProductId")),
                            Qty = reader.GetInt32(reader.GetOrdinal("Qty")),
                            PricePerUnit = reader.GetDecimal(reader.GetOrdinal("PricePerUnit")),
                            IsGift = reader.GetBoolean(reader.GetOrdinal("IsGift"))
                        });
                    }
                }
            }

        }
        catch (Exception)
        {

            throw;
        }
        return InvoiceItems;
    }

    public bool Update(InvoiceItem entity)
    {
        int rowAffected = 0;
        try
        {
            using (SqlCommand command = new SqlCommand("UpdateInvoiceItem", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@InvoiceItemId", entity.InvoiceItemId);
                command.Parameters.AddWithValue("@InvoiceId", entity.InvoiceId);
                command.Parameters.AddWithValue("@ProductId", entity.ProductId);
                command.Parameters.AddWithValue("@Qty", entity.Qty);
                command.Parameters.AddWithValue("@PricePerUnit", entity.PricePerUnit);
                command.Parameters.AddWithValue("@IsGift", entity.IsGift);

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
