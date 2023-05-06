using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRates.Shared.Extensions;

public static class DIExtension
{
    public static IHostBuilder AddSettingsConfiguration(this IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(configuration => { configuration.AddEnvironmentVariables(); });

        builder.ConfigureServices((hostContext, sc) =>
        {
            sc.Configure<EnvironmentSettings>(hostContext.Configuration
                .GetSection(EnvironmentSettings.SectionName));
            sc.AddSingleton<EnvironmentSettings>();
        });

        return builder;
    }
}