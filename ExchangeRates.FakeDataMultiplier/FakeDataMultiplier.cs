using System.ComponentModel;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRates.FakeDataMultiplier;

public class FakeDataMultiplier:BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public FakeDataMultiplier(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var repository= scope.ServiceProvider.GetRequiredService<IRepository<BankCurrenciesExchangeRatesEntity>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var data = repository.Query().OrderByDescending(x=>x.CreateDate).AsNoTracking().FirstAsync(cancellationToken: stoppingToken);

        foreach (var i in Enumerable.Range(1,30))
        {
            
        }



        await unitOfWork.SaveAsync();
    }
}