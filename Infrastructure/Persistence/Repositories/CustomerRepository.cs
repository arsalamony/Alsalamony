using Application.Common.Interfaces.Repositories;
using Application.Contracts.Customer;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        private SqlConnection connection;
        private SqlTransaction transaction;

        public CustomerRepository(SqlConnection connection, SqlTransaction transaction)
        {
            this.connection = connection;
            this.transaction = transaction;
        }

        public async Task<IEnumerable<CustomerViewResponse>> GetAllCustomers()
        {
            List<CustomerViewResponse> customers = new List<CustomerViewResponse>();

            using (SqlCommand command = ReposHelper.CreateCommand("SP_GetAllCustomers", connection, transaction))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;

                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        customers.Add(new CustomerViewResponse
                        {
                            CustomerId = reader.GetInt32(reader.GetOrdinal("CustomerId")),
                            CustomerName = reader.GetString(reader.GetOrdinal("CustomerName")),
                            Phone = reader.GetString(reader.GetOrdinal("Phone")),
                            Address = reader.GetString(reader.GetOrdinal("AddressName")),
                            Dept = reader.GetDecimal(reader.GetOrdinal("Dept"))
                        });
                    }
                }
            }

            return customers;
        }

        public async Task<bool> Add(Customer entity)
        {
            bool isAdded = false;

            using (SqlCommand command = ReposHelper.CreateCommand("SP_AddCustomer", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CustomerName", entity.CustomerName);
                command.Parameters.AddWithValue("@Phone", entity.Phone);
                command.Parameters.AddWithValue("@AddressId", entity.AddressId);
                command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.Int)
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
                    entity.CustomerId = (int)command.Parameters["@CustomerId"].Value;
                }
            }

            return isAdded;
        }

        public async Task<Customer> Find(int id)
        {
            Customer Customer = null;

            using (SqlCommand command = ReposHelper.CreateCommand("SP_GetCustomerById", connection, transaction))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CustomerId", id);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // The record was found
                        Customer = new Customer
                        {
                            CustomerId = id,
                            CustomerName = (string)reader["CustomerName"],
                            Phone = (string)reader["Phone"],
                            AddressId = (int)reader["AddressId"]
                        };

                    }

                }
            }

            return Customer;
        }

        public async Task<bool> Update(Customer entity)
        {
            int rowAffected = 0;

            using (SqlCommand command = ReposHelper.CreateCommand("SP_UpdateCustomer", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
                command.Parameters.AddWithValue("@CustomerName", entity.CustomerName);
                command.Parameters.AddWithValue("@Phone", entity.Phone);
                command.Parameters.AddWithValue("@AddressId", entity.AddressId);

                rowAffected = await command.ExecuteNonQueryAsync();
            }

            return rowAffected > 0;
        }

        public async Task<bool> Delete(int Id)
        {
            int rowAffected = 0;

            using (SqlCommand command = ReposHelper.CreateCommand("SP_DeleteCustomer", connection, transaction))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CustomerId", Id);


                rowAffected = await command.ExecuteNonQueryAsync();
            }

            return rowAffected > 0;
        }
    }
}
