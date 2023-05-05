using System.ComponentModel.DataAnnotations;

namespace ExchangeRates.Domain.Abstractions
{
    public abstract class Entity
    {
        [Key]
        public int Id { get; protected init; }

        public DateTime CreateDate { get; init; } = DateTime.Now;

        public DateOnly Date { get; init; } = DateOnly.FromDateTime(DateTime.Now);
    }
}