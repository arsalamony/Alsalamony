using Application.Common.Errors;
using Application.Common.Interfaces;
using Application.Common.Results;
using Application.Contracts.Customer;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Services.Customer
{
    public class CustomerServices : ICustomerServices
    {
        private readonly IUnitOfWork _UnitOfWork;

        public CustomerServices(IUnitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }


        public Result<CustomerResponse> Get(int id)
        {
            var customer = _UnitOfWork.CustomerRepository.Find(id);

            if (customer == null)
                return Result.Failure<CustomerResponse>(CustomerErrors.CustomerNotFound);
            

            // I Will Edit Mapping Later
            return Result.Success(new CustomerResponse
            {
                Id = customer.CustomerId,
                Name = customer.CustomerName,
                Phone = customer.Phone,
                AddressId = customer.AddressId,
            });
        }

        public Result<IEnumerable<CustomerViewResponse>> GetAllCustomers()
        {
            return  Result.Success(_UnitOfWork.CustomerRepository.GetAllCustomers());
        }

        public Result<CustomerResponse> Add(AddCustomerRequest request)
        {


            Domain.Entities.Customer newCustomer = new Domain.Entities.Customer 
            {
                CustomerName = request.Name,
                Phone = request.Phone,
                AddressId = request.AddressId
            };


            if (!_UnitOfWork.CustomerRepository.Add(newCustomer)) 
            {
                _UnitOfWork.Rollback();
                return Result.Failure<CustomerResponse>(new Error("Saving Customer", "Error Occured While Saving Customer", 500));
            }


            _UnitOfWork.Commit();
            return Result.Success(new CustomerResponse { Id = newCustomer.CustomerId, Name = newCustomer.CustomerName , Phone = newCustomer.Phone});
        }

        public Result Update(int Id, UpdateCustomerRequest request)
        {
            
            var customer = _UnitOfWork.CustomerRepository.Find(Id);

            if (customer == null)
                return Result.Failure(CustomerErrors.CustomerNotFound);

            customer.CustomerName = request.Name;
            customer.Phone = request.Phone;
            customer.AddressId = request.AddressId;

            if (!_UnitOfWork.CustomerRepository.Update(customer)) { 
                _UnitOfWork.Rollback();
                return Result.Failure(new Error("Updating Customer", "Error Occured While Updating Customer", 500));
            }

            _UnitOfWork.Commit();

            return Result.Success();
        }

        public Result Delete(int Id)
        {
            var r = _UnitOfWork.CustomerRepository.Delete(Id);
            if (!r)
            {
                _UnitOfWork.Rollback();
                return Result.Failure(new Error("Deleting Customer", "Error Occured While Deleting Customer", 500));
            }

            _UnitOfWork.Commit();
            return Result.Success();
        }
    }
}
