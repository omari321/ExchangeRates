using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.Services.Banks;

public class TbcBank : BankAbstract, IWebsiteParser
{
    private readonly ILogger<TbcBank> _logger;

    public TbcBank(ILogger<TbcBank> logger)
    {
        _logger = logger;
    }

    public async Task<ExchangeRate> GetExchangeRateAsync()
    {
        return await RetryService.Execute(ProcessAsync, BankNamesConst.TbcBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsync(string bankName)
    {
        var data = new ExchangeRate(BankNamesConst.TbcBank);

        const string html = "https://www.tbcbank.ge/web/ka/web/guest/exchange-rates?p_p_id=exchangeratessmall_WAR_tbcpwexchangeratessmallportlet&p_p_lifecycle=0&p_p_state=normal&p_p_mode=view&p_p_col_id=column-5&p_p_col_count=1";
        HtmlWeb web = new();
        var htmlDoc = await web.LoadFromWebAsync(html);
        var currencies = htmlDoc.DocumentNode.SelectNodes("//div[contains (@class,'currRate')]");
        var fetchedCurrencies = new List<string>()
        {
            "USD",
            "EUR",
            "GBP",
        };
        for (var i = 0; i < fetchedCurrencies.Count; i++)
        {
            try
            {
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        fetchedCurrencies[i],
                        decimal.Parse(currencies[i * 3 + 1].InnerText.Trim()),
                        decimal.Parse(currencies[i * 3 + 2].InnerText.Trim())));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Tbc Bank");
                _logger.LogError(e.Message);
            }
        }
        return data;
        //alt k + c code snippets
    }
}