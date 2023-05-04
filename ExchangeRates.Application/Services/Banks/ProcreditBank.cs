using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.Services.Banks;

internal class ProcreditBank : BankAbstract, IWebsiteParser
{
    private readonly ILogger<ProcreditBank> _logger;

    public ProcreditBank(ILogger<ProcreditBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.ExecuteAsync(ProcessAsyncAsync, BankNamesConst.ProcreditBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsyncAsync(string bankName)
    {
        var data = new ExchangeRate(bankName);
        const string html = "https://procreditbank.ge/ge/exchange";
        HtmlWeb web = new();
        var htmlDoc = await web.LoadFromWebAsync(html);
        var currenciesToFetch = new List<string>()
        {
            "USD",
            "EUR",
            "GBP",
            "RUR",
            "CHF"
        };
        var currenciesBuy = htmlDoc.DocumentNode.SelectNodes("//div[contains (@class,'exchange-buy')]");
        var currenciesSell = htmlDoc.DocumentNode.SelectNodes("//div[contains (@class,'exchange-sell')]");
        for (var i = 0; i < currenciesToFetch.Count; i++)
        {
            try
            {
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        currenciesToFetch[i],
                        decimal.Parse(currenciesBuy[i].InnerText.Trim()),
                        decimal.Parse(currenciesSell[i].InnerText.Trim())));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Procredit Bank");
                _logger.LogError(e.Message);
            }
        }

        return data;
    }
}