namespace ExchangeRates.Domain.Entities;

public class EntityExchangeRateInformation
{
    public string CurrencyName { get; set; } = default!;
    public decimal BuyRate { get; set; }
    public decimal SellRate { get; set; }
    public decimal OfficialRate { get; set; }
    public decimal Diff { get; set; }

    public EntityExchangeRateInformation(string currencyName, decimal buyRate, decimal sellRate, decimal officialRate, decimal diff)
    {
        CurrencyName = currencyName;
        BuyRate = buyRate;
        SellRate = sellRate;
        OfficialRate = officialRate;
        Diff = diff;
    }
}