﻿namespace ExchangeRates.Shared.Extensions;

public static class EnumExt
{
    public static string GetCurrencyNameFromEnum(this AvailableCurrencies availableCurrencies)
    {
        return availableCurrencies switch
        {
            AvailableCurrencies.CHF => "CHF",
            AvailableCurrencies.EUR => "EUR",
            AvailableCurrencies.GBP => "GBP",
            AvailableCurrencies.RUB => "RUR",
            AvailableCurrencies.USD => "USD",
            AvailableCurrencies.TRY => "TRY",
            _ => throw new ArgumentOutOfRangeException(nameof(availableCurrencies), availableCurrencies, null)
        };
    }
}