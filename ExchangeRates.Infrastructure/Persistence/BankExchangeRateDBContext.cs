using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Persistence.Configurations;
using ExchangeRates.Infrastructure.Persistence.Configurations.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Infrastructure.Persistence
{
    public class BankExchangeRateDBContext : DbContext
    {
        public BankExchangeRateDBContext()
        {
        }

        public BankExchangeRateDBContext(DbContextOptions<BankExchangeRateDBContext> options)
            : base(options)
        {
            //AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddConfiguration(new BankCurrenciesExchangeRateConfiguration());
        }

        public DbSet<BankCurrenciesExchangeRates> BankCurrenciesExchangeRates { get; set; }
    }
}