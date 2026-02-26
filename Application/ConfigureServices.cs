using Application.Services.Address;
using Application.Services.Auth;
using Application.Services.Customer;
using Application.Services.Invoice;
using Application.Services.InvoiceItem;
using Application.Services.Payment;
using Application.Services.Product;
using Application.Services.Report;
using Application.Services.SystemRecord;
using Application.Services.Task;
using Application.Services.User;
using Application.Services.UserProduct;
using Microsoft.Extensions.DependencyInjection;

namespace Application;



public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerServices, CustomerServices>();
        services.AddScoped<IProductServices, ProductServices>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IAddressServices, AddressServices>();
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IInvoiceServices, InvoiceServices>();
        services.AddScoped<IPaymentServices, PaymentServices>();
        services.AddScoped<IUserProductServices, UserProductServices>();
        services.AddScoped<ITaskServices, TaskServices>();
        services.AddScoped<IInvoiceItemSerivces, InvoiceItemServices>();
        services.AddScoped<ISystemRecordServices, SystemRecordServices>();
        services.AddScoped<IReportServices, ReportServices>();

        //services.AddMapsterConfig();
        //services.AddFluentValidationConfig();

        return services;
    }

    //private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
    //{
    //    //add Mapster
    //    var mappingCong = TypeAdapterConfig.GlobalSettings;
    //    mappingCong.Scan(Assembly.GetExecutingAssembly());



    //    services.AddSingleton<IMapper>(new Mapper(mappingCong));

    //    return services;
    //}

    //private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
    //{
    //    services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    //    services.AddFluentValidationAutoValidation();
    //    return services;
    //}
}
