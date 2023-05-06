using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using ExchangeRates.Shared;
using ExchangeRates.Shared.Extensions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.Services.Banks;

public class IshBank : BankAbstract, IBankParser
{
    private readonly ILogger<IshBank> _logger;

    public IshBank(ILogger<IshBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.ExecuteAsync(ProcessAsyncAsync, BankNamesConst.IshBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsyncAsync(string bankName)
    {
        var data = new ExchangeRate(bankName);
        const string html = "http://isbank.ge/ka/business#";

        HtmlWeb web = new();

        var htmlDoc = await web.LoadFromWebAsync(html);

        var currenciesBuy = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'exchange__item-currency__courses-buy')]/div");
        var currenciesSell = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class,'exchange__item-currency__courses-sell')]/div");

        var currencyNames =
            htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'exchange__item-currency__name')]");
        for (var i = 0; i < currencyNames.Count; i++)
        {
            try
            {
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        currencyNames[i].InnerText.Trim().ToUpper().GetBankName(),
                        decimal.Parse(currenciesBuy[i * 3 + 1].InnerText.Trim()),
                        decimal.Parse(currenciesSell[i * 3 + 1].InnerText.Trim())));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Ish Bank");
                _logger.LogError(e.Message);
            }
        }

        return data;
    }
}