using ExchangeRates.Infrastructure.Persistence;

namespace ExchangeRates.Infrastructure;

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