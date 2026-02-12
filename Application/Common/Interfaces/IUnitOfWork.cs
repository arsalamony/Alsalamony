namespace Application.Common.Interfaces;
using Application.Common.Interfaces.Repositories;



public interface IUnitOfWork : IDisposable
{
    void Commit();
    void Rollback();
    IUserRepository UserRepository { get; }

    ICustomerRepository CustomerRepository { get; }

    IProductRepository ProductRepository { get; }

    IAddressRepository AddressRepository { get; }

    IUserProductRepository UserProductRepository { get; }

    IInvoiceRepository InvoiceRepository { get; }

    IInvoiceItemRepository InvoiceItemRepository { get; }

    IPaymentRepository PaymentRepository { get; }

    ITaskRepository TaskRepository { get; }

    ISystemRecordRepository SystemRecordRepository { get; }
}
