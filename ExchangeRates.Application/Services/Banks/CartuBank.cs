using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using ExchangeRates.Shared;
using ExchangeRates.Shared.Extensions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ExchangeRates.Application.Services.Banks;

public class CartuBank : BankAbstract, IBankParser
{
    private readonly ILogger<CartuBank> _logger;

    public CartuBank(ILogger<CartuBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.ExecuteAsync(ProcessAsyncAsync, BankNamesConst.CartuBank, _logger);
    }

    protected override Task<ExchangeRate> ProcessAsyncAsync(string bankName)
    {
        const string url = "https://www.cartubank.ge/";
        new DriverManager().SetUpDriver(new ChromeConfig());
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("headless");
        using var driver = new ChromeDriver(chromeOptions);

        var data = new ExchangeRate(bankName);
        driver.Navigate().GoToUrl(url);
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(driver.PageSource);
        var currenciesName = htmlDoc.DocumentNode.SelectNodes("//th[contains (@class,'name')]");
        var currenciesBuy = htmlDoc.DocumentNode.SelectNodes("//td[contains (@class,'buy')]");
        var currenciesSell = htmlDoc.DocumentNode.SelectNodes("//td[contains (@class,'sell')]");

        for (var i = 0; i < currenciesName.Count; i++)
        {
            try
            {
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        currenciesName[i].InnerText.Trim().ToUpper().GetBankName(),
                        decimal.Parse(currenciesBuy[i].InnerText.Trim()),
                        decimal.Parse(currenciesSell[i].InnerText.Trim())));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Cartu Bank");
                _logger.LogError(e.Message);
            }
        }
        return Task.FromResult(data);
    }
}