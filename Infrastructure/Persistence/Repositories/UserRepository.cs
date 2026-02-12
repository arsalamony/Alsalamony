using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{

    private SqlConnection connection;
    private SqlTransaction transaction;

    public UserRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }

    public IEnumerable<User> GetAll()
    {
        List<User> userProducts = new List<User>();

        using (SqlCommand command = new SqlCommand("GetAllUsers", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    userProducts.Add(new User
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        Name = (string)reader["Name"],
                        Username = (string)reader["Username"],
                        Password = (string)reader["Password"],
                        Role = (string)reader["Role"],
                        Latitude = reader["Latitude"] as decimal?,
                        Longitude = reader["Longitude"] as decimal?,
                        DateOfLastLocation = reader["DateOfLastLocation"] as DateTime?
                    });
                }
            }
        }


        return userProducts;
    }
    public User Find(int userId)
    {
        User user = null;

        using (SqlCommand command = new SqlCommand("SP_GetUserByUserId", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@UserId", userId);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    // The record was found
                    user = new User();

                    user.UserId = userId;
                    user.Name = (string)reader["Name"];
                    user.Username = (string)reader["Username"];
                    user.Password = (string)reader["Password"];
                    user.Role = (string)reader["Role"];
                    user.Latitude = reader["Latitude"] as decimal?;
                    user.Longitude = reader["Longitude"] as decimal?;
                    user.DateOfLastLocation = reader["DateOfLastLocation"] as DateTime?;

                }
            }
        }


        return user;
    }

    public User Find(string UserName)
    {
        User user = null;

        using (SqlCommand command = new SqlCommand("SP_GetUserByUsername", connection, transaction))
        {
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Username", UserName);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    // The record was found
                    user = new User
                    {
                        UserId = (int)reader["UserId"],
                        Name = (string)reader["Name"],
                        Username = UserName,
                        Password = (string)reader["Password"],
                        Role = (string)reader["Role"],
                        Latitude = reader["Latitude"] as decimal?,
                        Longitude = reader["Longitude"] as decimal?,
                        DateOfLastLocation = reader["DateOfLastLocation"] as DateTime?
                    };
                }
            }
        }
        return user;
    }

    public bool Add(User entity)
    {
        bool isAdded = false;

        using (SqlCommand command = new SqlCommand("AddUser", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Username", entity.Username);
            command.Parameters.AddWithValue("@Password", entity.Password);
            command.Parameters.AddWithValue("@Role", entity.Role);

            var lat = command.Parameters.Add("@Latitude", SqlDbType.Decimal);
            lat.Precision = 9; lat.Scale = 6;
            lat.Value = (object?)entity.Latitude ?? DBNull.Value;

            var lon = command.Parameters.Add("@Longitude", SqlDbType.Decimal);
            lon.Precision = 9; lon.Scale = 6;
            lon.Value = (object?)entity.Longitude ?? DBNull.Value;

            command.Parameters.AddWithValue("@DateOfLastLocation", (object?)entity.DateOfLastLocation ?? DBNull.Value);
            command.Parameters.Add(new SqlParameter("@UserId", SqlDbType.Int)
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
                entity.UserId = (int)command.Parameters["@UserId"].Value;
            }
        }

        return isAdded;
    }
    public bool Update(User entity)
    {
        int rowAffected = 0;

        using (SqlCommand command = new SqlCommand("SP_UpdateUser", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", entity.UserId);
            command.Parameters.AddWithValue("@Name", entity.Name);
            command.Parameters.AddWithValue("@Username", entity.Username);
            command.Parameters.AddWithValue("@Password", entity.Password);
            command.Parameters.AddWithValue("@Role", entity.Role);
            command.Parameters.AddWithValue("@Latitude", (object?)entity.Latitude ?? DBNull.Value);
            command.Parameters.AddWithValue("@Longitude", (object?)entity.Longitude ?? DBNull.Value);
            command.Parameters.AddWithValue("@DateOfLastLocation", (object?)entity.DateOfLastLocation ?? DBNull.Value);

            rowAffected = command.ExecuteNonQuery();
        }

        return rowAffected > 0;
    }

    public bool Delete(int Id)
    {
        int rowAffected = 0;

        using (SqlCommand command = new SqlCommand("SP_DeleteUser", connection, transaction))
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@UserId", Id);


            rowAffected = command.ExecuteNonQuery();
        }

        return rowAffected > 0;
    }

}
