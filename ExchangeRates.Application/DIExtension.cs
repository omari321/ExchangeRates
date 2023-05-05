using ExchangeRates.Application.Interface;
using ExchangeRates.Application.Services;
using ExchangeRates.Application.Services.Banks;
using Microsoft.Extensions.DependencyInjection;

namespace ExchangeRates.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IBankParser, TeraBank>();
        services.AddSingleton<IBankParser, TbcBank>();
        services.AddSingleton<IBankParser, SilkBank>();
        services.AddSingleton<IBankParser, ProcreditBank>();
        services.AddSingleton<IBankParser, PashaBank>();
        services.AddSingleton<IBankParser, LibertyBank>();
        services.AddSingleton<IBankParser, IshBank>();
        services.AddSingleton<IBankParser, HalykBank>();
        services.AddSingleton<IBankParser, CredoBank>();
        services.AddSingleton<IBankParser, CartuBank>();
        services.AddSingleton<IBankParser, BankOfGeorgia>();
        services.AddSingleton<INbgParser, NationalBank>();
        return services;
    }
}