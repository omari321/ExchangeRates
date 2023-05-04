using ExchangeRates.Application;
using ExchangeRates.BackgroundWorker.HostedService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddLogging();
        services.AddApplication();
        services.AddHostedService<ExchangeRateParser>();
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        var env = hostingContext.HostingEnvironment;

        config.AddEnvironmentVariables();
        config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    })
    .Build()
    .Run(); ;

//todo retry logic
//also fetch current offical as 5 th column
//add as ienumerable in DI
//axali mediatorit task when all

//todo repo
//todo logging
//todo entity comparisons interface
//todo erovnuli banki
//todo gadaitane servicebi