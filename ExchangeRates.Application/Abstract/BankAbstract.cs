using ExchangeRates.Application.Interface;

namespace ExchangeRates.Application.Abstract;

public abstract class BankAbstract
{
    protected abstract Task<ExchangeRate> ProcessAsyncAsync(string bankName);
}