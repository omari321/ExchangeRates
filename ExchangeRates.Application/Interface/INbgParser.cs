using ExchangeRates.Application.Models;

namespace ExchangeRates.Application.Interface;

public interface INbgParser
{
    Task<NbgCurrencies> GetOfficialExchangeRateAsync();
}