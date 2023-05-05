using ExchangeRates.Domain.Abstractions;
using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using System.Linq.Expressions;

namespace ExchangeRates.Infrastructure.Repositories
{
    public class EfRepository<T> : IRepository<T>
        where T : Entity
    {
        private DBContext _db;

        public EfRepository(DBContext db)
        {
            _db = db;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>>? expression = null)
        {
            return _db.Set<T>().AsQueryable();
        }

        public virtual async Task Store(T document)
        {
            await _db.Set<T>().AddAsync(document);
        }
    }
}