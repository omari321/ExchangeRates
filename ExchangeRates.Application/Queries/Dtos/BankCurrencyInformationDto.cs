namespace ExchangeRates.Application.Queries.Dtos;

public class BankCurrencyInformationDto
{
    public string BankName { get; }
    public decimal BuyRate { get; }
    public decimal SellRate { get; }

    public BankCurrencyInformationDto(string bankName, decimal buyRate, decimal sellRate)
    {
        BankName = bankName;
        BuyRate = buyRate;
        SellRate = sellRate;
    }
}