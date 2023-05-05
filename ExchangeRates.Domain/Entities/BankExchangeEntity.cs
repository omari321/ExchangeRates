namespace ExchangeRates.Domain.Entities;

public class BankExchangeEntity
{
    public BankExchangeEntity(string bank)
    {
        Bank = bank;
    }

    public string Bank { get; set; } = default!;
    public List<EntityExchangeRateInformation>? ExchangeRates { get; set; } = default!;
}