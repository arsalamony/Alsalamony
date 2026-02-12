using Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;


namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddDbContext<AppDbContext>(options =>
        //{
        //    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
        //    builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
        //});

        //services.AddScoped<IAppDbContext, AppDbContext>();

        // Unit of Work
        services.AddScoped<IUnitOfWork>(provider => 
        {
            
            var connectionString = configuration.GetConnectionString("DefaultConnection")!;
            return new UnitOfWork(connectionString);
        });

        return services;
    }
}
