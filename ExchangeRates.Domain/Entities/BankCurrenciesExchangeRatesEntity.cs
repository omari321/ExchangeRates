using ExchangeRates.Domain.Abstractions;

namespace ExchangeRates.Domain.Entities;

public class BankCurrenciesExchangeRatesEntity : Entity
{
    public List<ExchangeRateEntity> CurrencyRatesInformation { get; } = default!;

    public BankCurrenciesExchangeRatesEntity()
    {
        CurrencyRatesInformation = new List<ExchangeRateEntity>();
    }
}