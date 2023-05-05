using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ExchangeRates.Application.Services;

public class NationalBank : INbgParser
{
    private readonly ILogger<NationalBank> _logger;

    public NationalBank(ILogger<NationalBank> logger)
    {
        _logger = logger;
    }

    protected async Task<NbgCurrencies> ProcessAsyncAsync(NbgCurrencies data)
    {
        var client = new HttpClient();

        _logger.LogInformation("trying to fetch data from nbg.gov.ge");
        var response =
            await client.GetAsync(
                $"https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/json/?date={DateOnly.FromDateTime(DateTime.Now)}");
        if (!response.IsSuccessStatusCode) return data;
        try
        {
            var content = await response.Content.ReadAsStringAsync();
            var items = JsonConvert.DeserializeObject<List<Root>>(content);
            var currenciesToFetch = new List<string>()
            {
                "USD",
                "EUR",
                "GBP",
                "RUB",
                "CHF",
                "TRY"
            };
            foreach (var currencyToFetch in currenciesToFetch.Select(currency =>
                         items![0].currencies.FirstOrDefault(x => x.code == currency)))
            {
                data.Currencies.Add(
                    new OfficialNbgCurrency(currencyToFetch!.code == "RUB" ? currencyToFetch.code : "RUR",
                        currencyToFetch.rate, currencyToFetch.diff));
            }
        }
        catch (Exception e)
        {
            _logger.LogError("failed to parse data");
            _logger.LogError(e.Message);
        }

        return data;
    }

    public Task<NbgCurrencies> GetOfficialExchangeRateAsync()
    {
        return RetryService.ExecuteAsync(ProcessAsyncAsync, new NbgCurrencies(BankNamesConst.NationalBank), _logger);
    }

    internal class Currency
    {
        public string code { get; set; } = default!;
        public int quantity { get; set; } = default!;
        public string rateFormated { get; set; } = default!;
        public string diffFormated { get; set; } = default!;
        public decimal rate { get; set; } = default!;
        public string name { get; set; } = default!;
        public decimal diff { get; set; } = default!;
        public DateTime date { get; set; } = default!;
        public DateTime validFromDate { get; set; } = default!;
    }

    internal class Root
    {
        public DateTime date { get; set; }
        public List<Currency> currencies { get; set; }
    }
}