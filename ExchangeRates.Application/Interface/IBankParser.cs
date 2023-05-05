using ExchangeRates.Application.Models;

namespace ExchangeRates.Application.Interface;

public interface IBankParser
{
    Task<ExchangeRate> GetExchangeRateAsync();
}