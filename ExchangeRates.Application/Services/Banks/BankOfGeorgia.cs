using ExchangeRates.Application.Abstract;
using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExchangeRates.Application.Services.Banks;

public class BankOfGeorgia : BankAbstract, IBankParser
{
    private readonly ILogger<BankOfGeorgia> _logger;

    public BankOfGeorgia(ILogger<BankOfGeorgia> logger)
    {
        _logger = logger;
    }

    public Task<ExchangeRate> GetExchangeRateAsync()
    {
        return RetryService.ExecuteAsync(ProcessAsyncAsync, BankNamesConst.BankOfGeorgia, _logger);
    }

    protected override async Task<ExchangeRate> ProcessAsyncAsync(string bankName)
    {
        var data = new ExchangeRate(BankNamesConst.BankOfGeorgia);
        var client = new HttpClient();//todo httpclientfactory
        var response = await client.GetAsync("https://bankofgeorgia.ge/api/currencies/page/pages");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var post = JsonConvert.DeserializeObject<BogExchangeRateResponseData>(content);
            var currenciesToFetch = new List<string>()
            {
                "USD",
                "EUR",
                "GBP",
                "RUR",
                "CHF"
            };
            foreach (var currency in currenciesToFetch)
            {
                var currencyToFetch = post!.Data.FirstOrDefault(x => x.Ccy == currency);
                try
                {
                    if (currency == "RUR")
                    {
                        currencyToFetch!.BuyRate = currencyToFetch.BuyRate / 100;
                        currencyToFetch.SellRate = currencyToFetch.SellRate / 100;
                    }
                    data.ExchangeRates.Add(
                        new ExchangeRateInformation(
                            currency!,
                            decimal.Parse(currencyToFetch!?.BuyRate!.ToString()!.Trim()!),
                            decimal.Parse(currencyToFetch?.SellRate.ToString()!.Trim()!)));
                }
                catch (Exception e)
                {
                    _logger.LogError("failed to fetch ExchangeRates for BoG Bank");
                    _logger.LogError(e.Message);
                }
            }
        }

        return data;
    }
}

internal class BogExchangeRateResponse
{
    public string Ccy { get; set; } = default!;
    public string ViewCcy { get; set; } = default!;
    public string DictionaryKey { get; set; } = default!;
    public decimal? RateWeight { get; set; }
    public decimal? PreviousRate { get; set; }
    public decimal? CurrentRate { get; set; }
    public decimal? Difference { get; set; }
    public decimal? BuyRate { get; set; }
    public decimal? SellRate { get; set; }
    public decimal? DgtlBuyRate { get; set; }
    public decimal? DgtlSellRate { get; set; }
    public decimal? MidBuyRate { get; set; }
    public decimal? MidSellRate { get; set; }
    public bool DefaultCcy { get; set; }
    public string Name { get; set; } = default!;
    public string Color { get; set; } = default!;
}

internal class BogExchangeRateResponseData
{
    public List<BogExchangeRateResponse> Data { get; set; } = default!;
}