using ExchangeRates.Domain.Abstractions;

namespace ExchangeRates.Domain.Entities;

public class BankCurrenciesExchangeRates : Entity
{
    public List<BankExchangeEntity> BankCurrencies { get; } = default!;

    public BankCurrenciesExchangeRates()
    {
        BankCurrencies = new List<BankExchangeEntity>();
    }
}