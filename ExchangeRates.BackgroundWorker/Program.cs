using ExchangeRates.Application;
using ExchangeRates.BackgroundWorker.HostedService;
using ExchangeRates.Infrastructure;
using ExchangeRates.Infrastructure.Persistence;
using ExchangeRates.Shared;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateDefaultBuilder(args);
var app = builder
.AddSettingsConfiguration()
.ConfigureServices((hostContext, services) =>
{
    services.AddLogging();
    services.AddApplication();
    services.AddDbContext();
    services.AddInfrastructure();
    services.AddHostedService<ExchangeRateParser>();
})
.Build();
app.InitializeDatabase();
app.Run(); ;

//todo retry logic
//also fetch current offical as 5 th column
//add as ienumerable in DI
//axali mediatorit task when all

//todo repo
//todo logging
//todo entity comparisons interface
//todo erovnuli banki
//todo gadaitane servicebi