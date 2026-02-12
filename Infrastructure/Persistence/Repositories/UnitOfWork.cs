using Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork
{
    SqlConnection _connection;
    SqlTransaction _transaction;

    public UnitOfWork(string ConStr)
    {
        _connection = new SqlConnection(ConStr);
        _connection.Open();

        _transaction = _connection.BeginTransaction();

    }

    public IUserRepository UserRepository => new UserRepository(_connection, _transaction);

    public ICustomerRepository CustomerRepository => new CustomerRepository(_connection, _transaction);

    public IProductRepository ProductRepository => new ProductRepository(_connection, _transaction);

    public IUserProductRepository UserProductRepository => new UserProductRepository(_connection, _transaction);

    public IInvoiceRepository InvoiceRepository =>  new InvoiceRepository(_connection, _transaction);

    public IInvoiceItemRepository InvoiceItemRepository => new InvoiceItemRepository(_connection, _transaction);

    public IPaymentRepository PaymentRepository => new PaymentRepository(_connection, _transaction);

    public IAddressRepository AddressRepository => new AddressRepository(_connection, _transaction);

    public ITaskRepository TaskRepository => new TaskRepository(_connection, _transaction);

    public ISystemRecordRepository SystemRecordRepository => new SystemRecordRepository(_connection, _transaction);

    public void Commit()
    {
        _transaction.Commit();
        _connection.Close();
        _connection.Dispose();
        _transaction.Dispose();
    }

    public void Rollback()
    {
        _transaction.Rollback();
        _connection.Close();
        _connection.Dispose();
        _transaction.Dispose();
    }

    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
        _transaction?.Dispose();
    }

}
