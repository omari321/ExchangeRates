using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Services.Banks;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRates.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IWebsiteParser, TeraBank>();
        services.AddSingleton<IWebsiteParser, TbcBank>();
        services.AddSingleton<IWebsiteParser, SilkBank>();
        services.AddSingleton<IWebsiteParser, ProcreditBank>();
        services.AddSingleton<IWebsiteParser, PashaBank>();
        services.AddSingleton<IWebsiteParser, LibertyBank>();
        services.AddSingleton<IWebsiteParser, IshBank>();
        services.AddSingleton<IWebsiteParser, HalykBank>();
        services.AddSingleton<IWebsiteParser, CredoBank>();
        services.AddSingleton<IWebsiteParser, CartuBank>();
        services.AddSingleton<IWebsiteParser, BankOfGeorgia>();

        return services;
    }
}