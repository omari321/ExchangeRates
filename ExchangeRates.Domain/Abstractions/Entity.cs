using System.ComponentModel.DataAnnotations;

namespace ExchangeRates.Domain.Abstractions
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; protected init; }

        public DateTimeOffset CreateDate { get; init; } = DateTimeOffset.Now.ToUniversalTime();
    }
}