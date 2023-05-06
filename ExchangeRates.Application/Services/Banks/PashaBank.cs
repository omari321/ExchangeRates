using System.Globalization;
using System.Text.RegularExpressions;
using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using ExchangeRates.Shared;
using ExchangeRates.Shared.Extensions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.Services.Banks;

public class PashaBank : BankAbstract, IBankParser
{
    private readonly ILogger<PashaBank> _logger;

    public PashaBank(ILogger<PashaBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.ExecuteAsync(ProcessAsyncAsync, BankNamesConst.PashaBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsyncAsync(string bankName)
    {
        var data = new ExchangeRate(bankName);
        const string html = "https://www.pashabank.ge/ge/exchange-rates";

        HtmlWeb web = new();

        var htmlDoc = await web.LoadFromWebAsync(html);

        var table = htmlDoc.DocumentNode.SelectSingleNode("//table");

        var nodes = table.SelectNodes("//tbody/tr");
        var regex = new Regex(@"\s+", RegexOptions.Compiled);
        var numberFormatWithComma = new NumberFormatInfo();
        numberFormatWithComma.NumberDecimalSeparator = ",";
        foreach (var node in nodes.ToList().GetRange(0, 4))
        {
            var values = regex.Split(node.InnerText.Trim());
            values = values.Where(x => x.Length > 0).ToArray();
            try
            {
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        values[0].GetBankName(),
                        decimal.Parse(values[1], numberFormatWithComma),
                        decimal.Parse(values[2], numberFormatWithComma)));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Pasha Bank");
                _logger.LogError(e.Message);
            }
        }

        return data;
    }
}