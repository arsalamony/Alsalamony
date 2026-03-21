using Application.Common.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Persistence.Repositories;

public class AddressRepository : IAddressRepository
{
    private SqlConnection connection;
    private SqlTransaction transaction;

    public AddressRepository(SqlConnection connection, SqlTransaction transaction)
    {
        this.connection = connection;
        this.transaction = transaction;
    }
    public Task<bool> Add(Address entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Address> Find(int id)
    {
        Address Address = null;

        using (SqlCommand command = ReposHelper.CreateCommand("Select * from Addresses where AddressId = @AddressId;", connection, transaction))
        {
            command.Parameters.AddWithValue("@AddressId", id);
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    // The record was found
                    Address = new Address
                    {
                        AddressID = id,
                        AddressName = (string)reader["AddressName"]
                    };

                }

            }
        }

        return Address;
    }

    public Task<bool> Update(Address entity)
    {
        throw new NotImplementedException();
    }
}
