using ExchangeRates.Application.Interface;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using ExchangeRates.Infrastructure.UnitOfWork.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.BackgroundWorker.HostedService;

public class ExchangeRateParser : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExchangeRateParser> _logger;
    private const int IntervalsInHours = 1;//todo

    public ExchangeRateParser(IServiceProvider serviceProvider, ILogger<ExchangeRateParser> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Processing batches in every {M} hours", 1);

        using var periodicTimer = new PeriodicTimer(TimeSpan.FromHours(IntervalsInHours));

        await StartWebsiteParsingAsync().ConfigureAwait(false);

        while (await periodicTimer.WaitForNextTickAsync(stoppingToken))
        {
            await StartWebsiteParsingAsync().ConfigureAwait(false);
        }
    }

    private async Task StartWebsiteParsingAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var nbgParser = scope.ServiceProvider.GetRequiredService<INbgParser>();
        var bankParsers = scope.ServiceProvider.GetRequiredService<IEnumerable<IBankParser>>(); ;
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<BankCurrenciesExchangeRates>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var bankData = await Task.WhenAll(
            bankParsers.Select(async (x) => await x.GetExchangeRateAsync()));
        var nbgData = await nbgParser.GetOfficialExchangeRateAsync();
        var persistenceData = new BankCurrenciesExchangeRates();
        foreach (var item in bankData)
        {
            Console.WriteLine(item.Bank);
            item.ExchangeRates.ForEach(x =>
            {
                Console.WriteLine(" " + x!.CurrencyName);
                Console.WriteLine("    " + x.BuyRate);
                Console.WriteLine("    " + x.SellRate);
            });
            var exchangeEntity = new BankExchangeEntity(item.Bank);
            if (item.ExchangeRates is { Count: > 0 })
            {
                exchangeEntity.ExchangeRates = new List<EntityExchangeRateInformation>();
                item.ExchangeRates.ForEach(x =>
                {
                    var officialData = nbgData.Currencies
                        .FirstOrDefault(y => y!.Name == x.CurrencyName);
                    exchangeEntity.ExchangeRates.Add(
                        new EntityExchangeRateInformation(
                                x!.CurrencyName,
                                x.BuyRate,
                                x.SellRate,
                                officialData!.Rate,
                                officialData.Rate
                            )
                        );
                });
            }

            persistenceData.BankCurrencies.Add(exchangeEntity);
        }

        await repository.Store(persistenceData);
        await unitOfWork.SaveAsync();
    }
}