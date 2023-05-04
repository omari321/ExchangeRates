using System.Text.RegularExpressions;
using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.Services.Banks;

public class LibertyBank : BankAbstract, IWebsiteParser
{
    private readonly ILogger<LibertyBank> _logger;

    public LibertyBank(ILogger<LibertyBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.Execute(ProcessAsync, BankNamesConst.LibertyBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsync(string bankName)
    {
        var data = new ExchangeRate(bankName);

        const string html = "https://libertybank.ge/ka/";
        HtmlWeb web = new();
        var htmlDoc = await web.LoadFromWebAsync(html);
        var elements = htmlDoc.DocumentNode.SelectNodes("//div[contains (@class,'currency-rates__row') and contains(@class,'js-homepage__currency-item')]");
        var regex = new Regex(@"\s+", RegexOptions.Compiled);
        foreach (var (t, index) in elements.Select((item, index) => (item, index)))
        {
            var items = regex.Split(t.InnerText.Trim());
            decimal? buy = null;
            decimal? sell = null;

            try
            {
                if (items[0] == "RUR")
                {
                    buy = decimal.Parse(items[7].Trim()) / 100;
                    sell = decimal.Parse(items[8].Trim()) / 100;
                }
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        items[0],
                        buy!.Value,
                        sell!.Value));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Liberty Bank");
                _logger.LogError(e.Message);
            }

            if (index > 4)
            {
                break;
            }
        }

        return data;
    }
}