using ExchangeRates.Application;
using ExchangeRates.BackgroundWorker.HostedService;
using ExchangeRates.Infrastructure;
using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Shared;
using ExchangeRates.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
var app = builder
.AddSettingsConfiguration()
.ConfigureServices((_, services) =>
{
    services.AddLogging();
    services.AddBankParsers();
    services.AddDbContext();
    services.AddInfrastructure();
    services.AddHostedService<ExchangeRateParser>();
})
.Build();
app.InitializeDatabase();
app.Run(); ;

//axali mediatorit task when all

//todo repo
//todo logging
//todo entity comparisons interface
//todo erovnuli banki
//todo gadaitane servicebi