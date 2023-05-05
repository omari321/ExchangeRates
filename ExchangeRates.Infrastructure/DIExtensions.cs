using ExchangeRates.Infrastructure.Repositories;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using ExchangeRates.Infrastructure.UnitOfWork.Abstractions;
using ExchangeRates.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRates.Infrastructure;

public static class DIExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<DbContext>((sp, options) =>
        {
            var appSettings = sp.GetRequiredService<EnvironmentSettings>();

            options.UseNpgsql(appSettings.DbConnection);
        });

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}