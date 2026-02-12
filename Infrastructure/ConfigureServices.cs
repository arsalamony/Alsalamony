using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Unit of Work
        services.AddScoped<IUnitOfWork>(provider => 
        {
            
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            return new UnitOfWork(connectionString);
        });

        return services;
    }
}
