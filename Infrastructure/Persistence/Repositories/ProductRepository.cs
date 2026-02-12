using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private SqlConnection connection;
    private SqlTransaction transaction;

    public ProductRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public IEnumerable<Product> GetAll()
    {
        List<Product> Products = new List<Product>();

        using (SqlCommand command = new SqlCommand("SP_GetAllProducts", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    // The record was found
                    Products.Add(new Product
                    {
                        ProductId = (int)reader["ProductId"],
                        ProductName = (string)reader["ProductName"],
                        Price = (decimal)reader["Price"]
                    });

                }

            }
        }

        return Products;
    }
    public Product Find(int id)
    {
        Product Product = null;

        using (SqlCommand command = new SqlCommand("SP_GetProductById", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@ProductId", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    // The record was found
                    Product = new Product
                    {
                        ProductId = id,
                        ProductName = (string)reader["ProductName"],
                        Price = (decimal)reader["Price"]
                    };

                }

            }
        }

        return Product;
    }

    public bool Add(Product entity)
    {
        bool isAdded = false;

        using (SqlCommand command = new SqlCommand("SP_AddProduct", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductName", entity.ProductName);
            command.Parameters.AddWithValue("@Price", entity.Price);
            command.Parameters.Add(new SqlParameter("@ProductId", SqlDbType.Int)
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
                entity.ProductId = (int)command.Parameters["@ProductId"].Value;
            }
        }

        return isAdded;
    }

    public bool Delete(int Id)
    {
        int rowAffected = 0;

        using (SqlCommand command = new SqlCommand("SP_DeleteProduct", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductId", Id);


            rowAffected = command.ExecuteNonQuery();
        }

        return rowAffected > 0;
    }

    public bool Update(Product entity)
    {
        int rowAffected = 0;

        using (SqlCommand command = new SqlCommand("SP_UpdateProduct", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@ProductName", entity.ProductName);
            command.Parameters.AddWithValue("@Price", entity.Price);

            rowAffected = command.ExecuteNonQuery();
        }

        return rowAffected > 0;
    }

}
