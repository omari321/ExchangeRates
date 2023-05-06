using System.Text.Json.Serialization;

namespace ExchangeRates.Shared;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AvailableCurrencies
{
    USD,
    EUR,
    GBP,
    RUB,
    CHF,
    TRY
}