using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExchangeRates.Infrastructure.Persistence.Configurations.Infrastructure
{
    public interface IEntityMappingConfiguration<T> : IEntityConfiguration
        where T : class
    {
        void Map(EntityTypeBuilder<T> builder);
    }
}