namespace ExchangeRates.Shared;

public class EnvironmentSettings
{
    public const string SectionName = "DbConnection";
    public string DbConnection { get; set; }

    public EnvironmentSettings(string dbConnection)
    {
        DbConnection = dbConnection;
    }

    private void Validate()
    {
        if (string.IsNullOrEmpty(DbConnection))
        {
            throw new ArgumentNullException(nameof(DbConnection));
        }
    }
}