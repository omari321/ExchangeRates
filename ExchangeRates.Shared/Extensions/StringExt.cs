namespace ExchangeRates.Shared.Extensions;

public static class StringExt
{
    public static string GetBankName(this string bank)
    {
        return bank == "RUB" ? "RUR" : bank;
    }
}