using ExchangeRates.FakeDataMultiplier;
using ExchangeRates.Infrastructure;
using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
var app = builder
    .AddSettingsConfiguration()
    .ConfigureServices((_, services) =>
    {
        services.AddDbContext();
        services.AddInfrastructure();
        services.AddHostedService<FakeDataMultiplier>();
    })
    .Build();
app.InitializeDatabase();
await app.RunAsync();