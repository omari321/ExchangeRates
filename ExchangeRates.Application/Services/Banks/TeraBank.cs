using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace ExchangeRates.Application.Services.Banks;

public class TeraBank : BankAbstract, IWebsiteParser
{
    private readonly ILogger<TeraBank> _logger;

    public TeraBank(ILogger<TeraBank> logger)
    {
        _logger = logger;
    }

    public async Task<ExchangeRate> GetExchangeRateAsync()
    {
        return await RetryService.Execute(ProcessAsync, BankNamesConst.TeraBank, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsync(string bankName)
    {
        //single page app
        //bin/Debug/net7.0/playwright.ps1 install in powershell
        var data = new ExchangeRate(BankNamesConst.TeraBank);
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true });
        var page = await browser.NewPageAsync();
        const string url = "https://www.terabank.ge/ge/retail";
        await page.GotoAsync(url);
        HtmlDocument htmlDoc = new();
        htmlDoc.LoadHtml(await page.ContentAsync());
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