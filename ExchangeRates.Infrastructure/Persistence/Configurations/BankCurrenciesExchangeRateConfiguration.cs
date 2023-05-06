using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Persistence.Configurations.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeRates.Infrastructure.Persistence.Configurations;

public class BankCurrenciesExchangeRateConfiguration : EntityConfiguration<BankCurrenciesExchangeRatesEntity>
{
    public override void Map(EntityTypeBuilder<BankCurrenciesExchangeRatesEntity> builder)
    {
        builder.ToTable("BankCurrenciesExchangeRates")
            .HasKey(x => x.Id);
        builder.Property(x => x.CurrencyRatesInformation).HasColumnType("jsonb");
    }
}