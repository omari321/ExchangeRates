using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ExchangeRates.Application.Services.Banks;

public class TeraBank : BankAbstract, IWebsiteParser
{
    private readonly ILogger<TeraBank> _logger;

    public TeraBank(ILogger<TeraBank> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.ExecuteAsync(ProcessAsyncAsync, BankNamesConst.TeraBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsyncAsync(string bankName)
    {
        const string url = "https://www.terabank.ge/ge/retail";
        var data = new ExchangeRate(BankNamesConst.TeraBank);
        new DriverManager().SetUpDriver(new ChromeConfig());
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("headless");
        using var driver = new ChromeDriver(chromeOptions);
        driver.Navigate().GoToUrl(url);
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(driver.PageSource);
        var currencies = htmlDoc.DocumentNode.SelectNodes("//table//tbody//span");
        for (var i = 0; i < currencies.Count / 3; i++)
        {
            try
            {
                data.ExchangeRates.Add(new ExchangeRateInformation(
                    currencies[i * 3].InnerText.Trim().ToUpper(),
                    decimal.Parse(currencies[i * 3 + 1].InnerText.Trim()),
                    decimal.Parse(currencies[i * 3 + 2].InnerText.Trim()))
                );
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Tera Bank");
                _logger.LogError(e.Message);
            }
        }
        return data;
    }
}