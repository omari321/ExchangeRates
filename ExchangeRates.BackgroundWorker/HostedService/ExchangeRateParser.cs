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
        var webParsers = scope.ServiceProvider.GetRequiredService<IEnumerable<IWebsiteParser>>();
        var data = await Task.WhenAll(
            webParsers.Select(async (x) => await x.GetExchangeRateAsync()));
        foreach (var item in data)
        {
            Console.WriteLine(item.Bank);
            item.ExchangeRates.ForEach(x =>
            {
                Console.WriteLine("    " + x.BuyRate);
                Console.WriteLine("    " + x.CurrencyName);
                Console.WriteLine("    " + x.SellRate);
            });
        }
    }
}