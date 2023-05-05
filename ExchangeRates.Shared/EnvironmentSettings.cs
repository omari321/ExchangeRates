namespace ExchangeRates.Shared;

public class EnvironmentSettings
{
    public const string SectionName = "EnvironmentSettings";
    public string DbConnection { get; set; }

    public EnvironmentSettings()
    {
        Validate();
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(DbConnection))
        {
            throw new ArgumentNullException(nameof(DbConnection));
        }
    }
}