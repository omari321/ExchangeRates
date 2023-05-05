using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Models;

namespace ExchangeRates.Application.Abstract;

public abstract class BankAbstract
{
    protected abstract Task<ExchangeRate> ProcessAsyncAsync(string bankName);
}