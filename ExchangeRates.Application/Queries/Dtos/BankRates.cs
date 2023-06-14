namespace ExchangeRates.Application.Queries.Dtos;

internal class BankRates
{
    public string BankName { get; set; }
    public decimal BuyRate { get; set; }
    public decimal SellRate { get; set; }

    public BankRates(string bankName, decimal buyRate, decimal sellRate)
    {
        BankName = bankName;
        BuyRate = buyRate;
        SellRate = sellRate;
    }
}