namespace ExchangeRates.Domain.Entities;

public class EntityExchangeRateInformation
{
    public string CurrencyName { get; set; } = default!;
    public decimal BuyRate { get; set; }
    public decimal SellRate { get; set; }
    public decimal OfficialRate { get; set; }
    public decimal Diff { get; set; }
}