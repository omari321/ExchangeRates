namespace ExchangeRates.Application.Models;

public class NbgCurrencies
{
    public string OrgName { get; init; }
    public List<OfficialNbgCurrency?> Currencies { get; init; }

    public NbgCurrencies(string orgName)
    {
        OrgName = orgName;
        Currencies = new List<OfficialNbgCurrency?>();
    }
}