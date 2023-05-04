namespace ExchangeRates.Application.Interface;

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

public record ExchangeRateInformation(string CurrencyName, decimal BuyRate, decimal SellRate);

public interface IWebsiteParser
{
    Task<ExchangeRate> GetExchangeRateAsync();
}