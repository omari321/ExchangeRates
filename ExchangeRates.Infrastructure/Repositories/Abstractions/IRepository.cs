using System.Linq.Expressions;
using ExchangeRates.Infrastructure.Persistence;

namespace ExchangeRates.Infrastructure.Repositories.Abstractions
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);

        Task Store(T document);
    }
}