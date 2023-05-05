using ExchangeRates.Application.Interface;
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
        var bankParsers = scope.ServiceProvider.GetRequiredService<IEnumerable<IBankParser>>();
        var bankData = await Task.WhenAll(
            bankParsers.Select(async (x) => await x.GetExchangeRateAsync()));
        var nbgData = await nbgParser.GetOfficialExchangeRateAsync();
        foreach (var item in bankData)
        {
            Console.WriteLine(item.Bank);
            item.ExchangeRates.ForEach(x =>
            {
                Console.WriteLine(" " + x!.CurrencyName);
                Console.WriteLine("    " + x.BuyRate);
                Console.WriteLine("    " + x.SellRate);
            });
        }
    }
}