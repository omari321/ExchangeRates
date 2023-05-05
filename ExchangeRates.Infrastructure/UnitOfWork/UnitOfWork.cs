using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Infrastructure.UnitOfWork.Abstractions;

namespace ExchangeRates.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly BankExchangeRateDBContext _bankExchangeRateDbContext;

    public UnitOfWork(BankExchangeRateDBContext bankExchangeRateDbContext)
    {
        _bankExchangeRateDbContext = bankExchangeRateDbContext;
    }

    public async Task<int> SaveAsync()
    {
        return await _bankExchangeRateDbContext.SaveChangesAsync();
    }
}