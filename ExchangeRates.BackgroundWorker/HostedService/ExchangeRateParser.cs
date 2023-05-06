using ExchangeRates.Application.Interface;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
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
        var repository = scope.ServiceProvider.GetRequiredService<IRepository<BankCurrenciesExchangeRatesEntity>>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var bankData = await Task.WhenAll(
            bankParsers.Select(async (x) => await x.GetExchangeRateAsync()));
        var nbgData = await nbgParser.GetOfficialExchangeRateAsync();
        var persistenceData = new BankCurrenciesExchangeRatesEntity();
        var dictionary = new Dictionary<(string Name, decimal OfficialRate, decimal Diff), List<EntityExchangeRateInformation>>();
        foreach (var item in bankData)
        {
            item.ExchangeRates.ForEach(x =>
            {
                var officialData = nbgData.Currencies
                    .FirstOrDefault(y => y!.Name == x.CurrencyName);
                var key = (x!.CurrencyName, officialData!.Rate, officialData.Diff);
                if (dictionary.ContainsKey(key))
                {
                    dictionary[key].Add(new EntityExchangeRateInformation(
                        item.Bank,
                        x.BuyRate,
                        x.SellRate
                    ));
                }
                else
                {
                    dictionary.Add(key, new List<EntityExchangeRateInformation>()
                    {
                        new(
                            item.Bank,
                            x.BuyRate,
                            x.SellRate
                        )
                    });
                }
            });
        }
        foreach (var dictionaryKey in dictionary.Keys)
        {
            var exchangeRate =
                new ExchangeRateEntity(dictionaryKey.Diff, dictionaryKey.OfficialRate, dictionaryKey.Name);
            dictionary[dictionaryKey].ForEach(x =>
            {
                exchangeRate.ExchangeRates?.Add(x);
            });
            persistenceData.CurrencyRatesInformation.Add(exchangeRate);
        }
        await repository.Store(persistenceData);
        await unitOfWork.SaveAsync();
        _logger.LogInformation("Saved Data Successfully");
    }
}