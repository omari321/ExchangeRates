using System.Text.RegularExpressions;
using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.Services.Banks;

public class SilkBank : BankAbstract, IWebsiteParser
{
    private readonly ILogger<SilkBank> _logger;

    public SilkBank(ILogger<SilkBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.Execute(ProcessAsync, BankNamesConst.SilkBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsync(string bankName)
    {
        var data = new ExchangeRate(bankName);
        const string html = "https://www.silkbank.ge/ka";
        HtmlWeb web = new();
        var htmlDoc = await web.LoadFromWebAsync(html);
        var currenciesToFetch = new List<string>()
        {
            "EUR",
            "GBP",
            "RUR",
            "USD"
        };
        var regexForDecimal = new Regex(@"\d+.+\d", RegexOptions.Compiled);
        var regexForEmpty = new Regex(@"\s+", RegexOptions.Compiled);
        foreach (var currency in currenciesToFetch)
        {
            var currencies = htmlDoc.DocumentNode.SelectSingleNode($"//div[contains(@class,'item') and @title='{currency}']");//div[contains(@class,'item')]");
            var values = regexForEmpty.Split(currencies.InnerText.Trim());
            try
            {
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        currency,
                        decimal.Parse(regexForDecimal.Match(values[1]).Value.Trim()),
                        decimal.Parse(regexForDecimal.Match(values[2]).Value.Trim())));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Silk Bank");
                _logger.LogError(e.Message);
            }
        }

        return data;
    }
}