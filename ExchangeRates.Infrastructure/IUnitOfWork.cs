namespace ExchangeRates.Infrastructure;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
}