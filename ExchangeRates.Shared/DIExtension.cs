using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRates.Shared;

public static class DIExtension
{
    public static IHostBuilder AddSettingsConfiguration(this IHostBuilder builder)
    {
        builder.ConfigureAppConfiguration(config =>
        {
            config.AddEnvironmentVariables();
        });

        builder.ConfigureServices((hostContext, sc) =>
        {
            sc.Configure<EnvironmentSettings>(hostContext.Configuration.GetSection(EnvironmentSettings.SectionName));

            sc.AddSingleton<EnvironmentSettings>();
            var settings = sc.BuildServiceProvider().GetRequiredService<EnvironmentSettings>();
        });

        return builder;
    }
}