using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Infrastructure.Persistence.Configurations.Infrastructure
{
    public interface IEntityConfiguration
    {
        void Map(ModelBuilder builder);
    }
}