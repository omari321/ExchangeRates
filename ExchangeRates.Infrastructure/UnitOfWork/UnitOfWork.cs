using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Infrastructure.UnitOfWork.Abstractions;

namespace ExchangeRates.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DBContext _dbContext;

    public UnitOfWork(DBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }
}