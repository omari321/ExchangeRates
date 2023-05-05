using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Infrastructure.Repositories;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using ExchangeRates.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRates.Infrastructure;

public static class DIExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContext<BankExchangeRateDBContext>((sp, options) =>
        {
            var appSettings = sp.GetRequiredService<EnvironmentSettings>();

            options.UseNpgsql("User ID=sa;Password=postgreS@pass1;Host=localhost;Port=5432;Database=BankExchangeRates;Pooling=true;");
        });
        services.AddScoped<BankExchangeRateDBContext>();
        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}