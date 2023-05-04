using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace ExchangeRates.Application.Services.Banks;

public class CartuBank : BankAbstract, IWebsiteParser
{
    private readonly ILogger<CartuBank> _logger;

    public CartuBank(ILogger<CartuBank> logger)
    {
        _logger = logger;
    }

    public async Task<ExchangeRate> GetExchangeRateAsync()
    {
        return await RetryService.Execute(ProcessAsync, BankNamesConst.CartuBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsync(string bankName)
    {
        const string url = "https://www.cartubank.ge/";
        new DriverManager().SetUpDriver(new ChromeConfig());
        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("headless");
        var driver = new ChromeDriver(chromeOptions);
        var data = new ExchangeRate(bankName);
        //using var playwright = await Playwright.CreateAsync();
        //await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
        //var page = await browser.NewPageAsync();
        //await page.GotoAsync(url);
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(await page.ContentAsync());
        var currenciesName = htmlDoc.DocumentNode.SelectNodes("//th[contains (@class,'name')]");
        var currenciesBuy = htmlDoc.DocumentNode.SelectNodes("//td[contains (@class,'buy')]");
        var currenciesSell = htmlDoc.DocumentNode.SelectNodes("//td[contains (@class,'sell')]");

        for (var i = 0; i < currenciesName.Count; i++)
        {
            try
            {
                data.ExchangeRates.Add(
                    new ExchangeRateInformation(
                        currenciesName[i].InnerText.Trim().ToUpper(),
                        decimal.Parse(currenciesBuy[i].InnerText.Trim()),
                        decimal.Parse(currenciesSell[i].InnerText.Trim())));
            }
            catch (Exception e)
            {
                _logger.LogError("failed to fetch ExchangeRates for Cartu Bank");
                _logger.LogError(e.Message);
            }
        }
        return data;
    }
}