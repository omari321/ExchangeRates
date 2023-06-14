namespace ExchangeRates.Application.Queries.Dtos;

internal class BankHistoricalRatesDto
{
    public DateOnly Date { get; set; }
    public List<BankRates> BankRates { get; set; }
}