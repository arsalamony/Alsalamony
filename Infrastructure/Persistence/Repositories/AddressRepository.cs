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
    public bool Add(Address entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Address Find(int id)
    {
        Address Address = null;

        using (SqlCommand command = new SqlCommand("Select * from Addresses where AddressId = @AddressId;", connection, transaction))
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

    public bool Update(Address entity)
    {
        throw new NotImplementedException();
    }
}
