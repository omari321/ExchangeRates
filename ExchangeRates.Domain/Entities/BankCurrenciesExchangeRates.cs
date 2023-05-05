using ExchangeRates.Domain.Abstractions;

namespace ExchangeRates.Domain.Entities;

public class BankCurrenciesExchangeRates : Entity
{
    public List<BankExchangeEntity> BankCurrencies { get; set; } = default!;
}