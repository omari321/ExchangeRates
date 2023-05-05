using ExchangeRates.Domain.Abstractions;
using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using System.Linq.Expressions;

namespace ExchangeRates.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T>
        where T : Entity
    {
        private BankExchangeRateDBContext _bankExchangeRateDb;

        public EfRepository(BankExchangeRateDBContext bankExchangeRateDb)
        {
            _bankExchangeRateDb = bankExchangeRateDb;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>>? expression = null)
        {
            return _bankExchangeRateDb.Set<T>().AsQueryable();
        }

        public virtual async Task Store(T document)
        {
            await _bankExchangeRateDb.Set<T>().AddAsync(document);
        }
    }
}