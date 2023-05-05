using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Infrastructure.Persistence;

public static class DBConfigExtensions
{
    public static async Task<IApplicationBuilder> InitializeDatabase(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BankExchangeRateDBContext>>();

        await using var ctx = scope.ServiceProvider.GetRequiredService<BankExchangeRateDBContext>();

        var pendingMigrations = await ctx.Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await ctx.Database.MigrateAsync();
        }

        return builder;
    }
}