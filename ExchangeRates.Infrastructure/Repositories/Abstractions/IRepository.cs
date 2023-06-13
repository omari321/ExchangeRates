using ExchangeRates.Domain.Abstractions;
using System.Linq.Expressions;

namespace ExchangeRates.Infrastructure.Repositories.Abstractions
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> Query(Expression<Func<T, bool>>? expression = null);

        Task Store(T document);
    }
}