namespace ExchangeRates.Application.Models;

public class ExchangeRate
{
    public string Bank;
    public List<ExchangeRateInformation?> ExchangeRates { get; }

    public ExchangeRate(string bank)
    {
        Bank = bank;
        ExchangeRates = new List<ExchangeRateInformation?>();
    }
}