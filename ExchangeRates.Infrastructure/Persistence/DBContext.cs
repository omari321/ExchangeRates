using ExchangeRates.Infrastructure.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Infrastructure.Persistence
{
    public class DBContext : DbContext
    {
        public DBContext()
        {
        }

        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}