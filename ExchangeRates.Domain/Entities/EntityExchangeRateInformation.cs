namespace ExchangeRates.Domain.Entities;

public class EntityExchangeRateInformation
{
    public string BankName { get; set; } = default!;
    public decimal BuyRate { get; set; }
    public decimal SellRate { get; set; }

    public EntityExchangeRateInformation(string bankName, decimal buyRate, decimal sellRate)
    {
        BankName = bankName;
        BuyRate = buyRate;
        SellRate = sellRate;
    }
}