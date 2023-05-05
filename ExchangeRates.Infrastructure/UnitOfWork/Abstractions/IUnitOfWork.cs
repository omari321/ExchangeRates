namespace ExchangeRates.Infrastructure.UnitOfWork.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveAsync();
}