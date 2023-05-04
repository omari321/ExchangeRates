using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.Services.Banks;

public class HalykBank : BankAbstract, IWebsiteParser
{
    private readonly ILogger<HalykBank> _logger;

    public HalykBank(ILogger<HalykBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.Execute(ProcessAsync, BankNamesConst.HalykBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsync(string bankName)
    {
        var data = new ExchangeRate(bankName);
        const string html = "https://halykbank.ge/ka/individuals";
        HtmlWeb web = new();
        var htmlDoc = await web.LoadFromWebAsync(html);
        var currencyNames = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'homepage__currencies-exchange-currency')]"); //div[contains(@class, '')]
        var currencies = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'homepage__currencies-exchange-price')]");//[@class='item']

        for (var i = 0; i < currencyNames.Count - 1; i++)
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
                _logger.LogError("failed to fetch ExchangeRates for Halyk Bank");
                _logger.LogError(e.Message);
            }
        }
        return data;
    }
}