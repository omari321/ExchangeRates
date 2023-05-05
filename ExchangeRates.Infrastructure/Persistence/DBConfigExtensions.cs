using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Infrastructure.Persistence;

public static class DBConfigExtensions
{
    public static IHost InitializeDatabase(this IHost builder)
    {
        using var scope = builder.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BankExchangeRateDBContext>>();

        using var ctx = scope.ServiceProvider.GetRequiredService<BankExchangeRateDBContext>();

        var pendingMigrations = ctx.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult();
        if (pendingMigrations.Any())
        {
            ctx.Database.MigrateAsync().GetAwaiter().GetResult();
        }

        return builder;
    }
}