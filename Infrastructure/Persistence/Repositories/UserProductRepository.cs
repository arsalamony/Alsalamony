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

    public async Task<UserProduct> Find(int UserProductId)
    {
        UserProduct userProduct = null;

        using (SqlCommand command = ReposHelper.CreateCommand("SP_GetUserProductByUserProductId", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserProductId", UserProductId);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
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

    public async Task<ICollection<UserProduct>> GetAll(int UserId)
    {
        List<UserProduct> userProducts = new List<UserProduct>();

        using (SqlCommand command = ReposHelper.CreateCommand("SP_GetAllUserProducts", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", UserId);

            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
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

    public async Task<bool> Add(UserProduct entity)
    {
        bool isAdded = false;

        using (SqlCommand command = ReposHelper.CreateCommand("SP_AddUserProduct", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductId", entity.ProductId);
            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Qty", entity.Qty);
            command.Parameters.Add(new SqlParameter("@UserProductId", SqlDbType.Int)
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
                entity.UserProductId = (int)command.Parameters["@UserProductId"].Value;
            }
        }

        return isAdded;
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }


    public async Task<bool> Update(UserProduct userProduct)
    {
        int rowsAffected = 0;

        using (SqlCommand command = ReposHelper.CreateCommand("SP_UpdateUserProduct", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserProductId", userProduct.UserProductId);
            command.Parameters.AddWithValue("@ProductId", userProduct.ProductId);
            command.Parameters.AddWithValue("@UserId", userProduct.UserId);
            command.Parameters.AddWithValue("@Qty", userProduct.Qty);

            rowsAffected = await command.ExecuteNonQueryAsync();
        }

        return rowsAffected > 0;
    }

}
