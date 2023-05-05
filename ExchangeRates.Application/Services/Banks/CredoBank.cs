using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.Services.Banks;

public class CredoBank : BankAbstract, IBankParser
{
    private readonly ILogger<CredoBank> _logger;

    public CredoBank(ILogger<CredoBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.ExecuteAsync(ProcessAsyncAsync, BankNamesConst.CredoBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsyncAsync(string bankName)
    {
        var data = new ExchangeRate(bankName);
        const string html = "https://credobank.ge/exchange-rates/?rate=credo";

        HtmlWeb web = new();

        var htmlDoc = await web.LoadFromWebAsync(html);
        var currencyNames = htmlDoc.DocumentNode.SelectNodes("//span[contains(@class, 'currency-code')]");
        var currencies = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'currency-rate-box')]/ul[2]/li/span[1]");
        for (var i = 0; i < currencyNames.Count; i++)
        {
            try
            {
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        currencyNames[i].InnerText.Trim().ToUpper(),
                        decimal.Parse(currencies[i * 2].InnerText.Trim()),
                        decimal.Parse(currencies[i * 2 + 1].InnerText.Trim())));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Credo Bank");
                _logger.LogError(e.Message);
            }
        }
        return data;
    }
}