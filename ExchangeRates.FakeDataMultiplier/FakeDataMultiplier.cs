using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRates.FakeDataMultiplier;

public class FakeDataMultiplier : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public FakeDataMultiplier(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<BankCurrenciesExchangeRatesEntity>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var data = await repository.Query()
            .OrderByDescending(x => x.CreateDate)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken: stoppingToken);
        if (data is not null)
        {



            foreach (var i in Enumerable.Range(1, 30))
            {
                var dataList = new List<ExchangeRateEntity>();

                data!.CurrencyRatesInformation.ForEach(x =>
                {


                    var random = Random.Shared.Next();
                    decimal multiplyValue;
                    if (random > 0)
                    {
                        multiplyValue = 1 + i / 100;
                    }
                    else
                    {
                        multiplyValue = 1 - i / 100;
                    }

                    var data = new ExchangeRateEntity(x.Diff * multiplyValue, x.OfficialRate * multiplyValue, x.CurrencyName);
                    x.ExchangeRates.ForEach(e =>
                    {
                        data.ExchangeRates.Add(new EntityExchangeRateInformation(e!.BankName, e.BuyRate * multiplyValue, e.SellRate * multiplyValue));
                    });
                    dataList.Add(data);
                });
                var currencies = new BankCurrenciesExchangeRatesEntity
                {
                    CreateDate = DateTimeOffset.Now.ToUniversalTime().AddDays(-i)
                };
                currencies.CurrencyRatesInformation.AddRange(dataList);
                await repository.Store(currencies);
            }




            await unitOfWork.SaveAsync();

        }
    }
}