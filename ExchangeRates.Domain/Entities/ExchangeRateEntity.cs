namespace ExchangeRates.Domain.Entities;

public class ExchangeRateEntity
{
    public ExchangeRateEntity(decimal diff, decimal officialRate, string currencyName)
    {
        Diff = diff;
        OfficialRate = officialRate;
        CurrencyName = currencyName;
        ExchangeRates = new List<EntityExchangeRateInformation?>();
    }

    public decimal Diff { get; set; }
    public decimal OfficialRate { get; set; }
    public string CurrencyName { get; set; }
    public List<EntityExchangeRateInformation?> ExchangeRates { get; set; } = default!;
}