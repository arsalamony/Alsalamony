using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Infrastructure.Persistence.Repositories;

public class UserProductRepository : IUserProductRepository
{
    private SqlConnection connection;
    private SqlTransaction transaction;

    public UserProductRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public UserProduct Find(int UserProductId)
    {
        UserProduct userProduct = null;

        using (SqlCommand command = new SqlCommand("SP_GetUserProductByUserProductId", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserProductId", UserProductId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    userProduct = new UserProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Qty = Convert.ToInt32(reader["Qty"]),
                        UserProductId = UserProductId,
                        UserId = Convert.ToInt32(reader["UserId"])
                    };
                }
            }
        }

        return userProduct;
    }

    public ICollection<UserProduct> GetAll(int UserId)
    {
        List<UserProduct> userProducts = new List<UserProduct>();

        using (SqlCommand command = new SqlCommand("SP_GetAllUserProducts", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", UserId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    userProducts.Add(new UserProduct
                    {
                        ProductId = Convert.ToInt32(reader["ProductId"]),
                        Qty = Convert.ToInt32(reader["Qty"]),
                        UserProductId = Convert.ToInt32(reader["UserProductId"]),
                        UserId = UserId
                    });
                }
            }
        }


        return userProducts;
    }

    public bool Add(UserProduct entity)
    {
        bool isAdded = false;

        using (SqlCommand command = new SqlCommand("SP_AddUserProduct", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Qty", entity.Qty);
            command.Parameters.Add(new SqlParameter("@UserProductId", SqlDbType.Int)
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
                entity.UserProductId = (int)command.Parameters["@UserProductId"].Value;
            }
        }

        return isAdded;
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }


    public bool Update(UserProduct userProduct)
    {
        int rowsAffected = 0;

        using (SqlCommand command = new SqlCommand("SP_UpdateUserProduct", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserProductId", userProduct.UserProductId);
            command.Parameters.AddWithValue("@ProductId", userProduct.ProductId);
            command.Parameters.AddWithValue("@UserId", userProduct.UserId);
            command.Parameters.AddWithValue("@Qty", userProduct.Qty);

            rowsAffected = command.ExecuteNonQuery();
        }

        return rowsAffected > 0;
    }

}
