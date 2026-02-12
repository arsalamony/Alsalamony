using Application.Common.Interfaces.Repositories;
using Application.Contracts.Customer;
using Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Data;

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

        public IEnumerable<CustomerViewResponse> GetAllCustomers()
        {
            List<CustomerViewResponse> customers = new List<CustomerViewResponse>();

            try
            {
                using (SqlCommand command = new SqlCommand("SP_GetAllCustomers", connection, transaction))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
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

            }
            catch (Exception)
            {

                throw;
            }

            return customers;
        }

        public bool Add(Customer entity)
        {
            bool isAdded = false;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_AddCustomer", connection, transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerName", entity.CustomerName);
                    command.Parameters.AddWithValue("@Phone", entity.Phone);
                    command.Parameters.AddWithValue("@AddressId", entity.AddressId);
                    command.Parameters.Add(new SqlParameter("@CustomerId", SqlDbType.Int)
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
                        entity.CustomerId = (int)command.Parameters["@CustomerId"].Value;
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

            return isAdded;
        }

        public Customer Find(int id)
        {

            Customer Customer = null;


            try
            {

                using (SqlCommand command = new SqlCommand("SP_GetCustomerById", connection, transaction))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CustomerId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
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

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }


            return Customer;
        }

        public bool Update(Customer entity)
        {
            int rowAffected = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_UpdateCustomer", connection, transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerId", entity.CustomerId);
                    command.Parameters.AddWithValue("@CustomerName", entity.CustomerName);
                    command.Parameters.AddWithValue("@Phone", entity.Phone);
                    command.Parameters.AddWithValue("@AddressId", entity.AddressId);

                    rowAffected = command.ExecuteNonQuery();
                }

            }
            catch (Exception)
            {

                throw;
            }

            return rowAffected > 0;
        }


        public bool Delete(int Id)
        {
            int rowAffected = 0;
            try
            {
                using (SqlCommand command = new SqlCommand("SP_DeleteCustomer", connection, transaction))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CustomerId", Id);


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
}
