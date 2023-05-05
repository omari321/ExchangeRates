using ExchangeRates.Application.Interface;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;
using ExchangeRates.Application.Models;

namespace ExchangeRates.Application.Services;

public static class RetryService
{
    public static async Task<ExchangeRate> ExecuteAsync(Func<string, Task<ExchangeRate>> func, string bankName, ILogger logger, int numberOfRetries = 2)
    {
        var tries = 0;

        while (tries <= numberOfRetries)
        {
            logger.LogInformation("getting ready to start parsing {0} data", bankName);
            try
            {
                var data = await func(bankName);
                if (data.ExchangeRates is not { Count: > 0 })
                {
                    logger.LogError("failed to parse  {0} data , error in logic", bankName);
                }
                return data;
            }
            catch
            {
                tries++;
                logger.LogError("failed to parse bank data {0} times", tries);
            }
            var delay = Random.Shared.Next(500, 2000);
            logger.LogInformation("waiting for {0} miliseconds", delay);
            await Task.Delay(delay);
        }

        return new ExchangeRate(bankName);
    }

    public static async Task<T> ExecuteAsync<T>(Func<T, Task<T>> func, T data, ILogger logger, int numberOfRetries = 2, [CallerArgumentExpression("func")] string? argumentName = default)
    {
        var tries = 0;

        while (tries <= numberOfRetries)
        {
            logger.LogInformation("getting ready to execute function {0}", argumentName);
            try
            {
                return await func(data);
            }
            catch
            {
                tries++;
                logger.LogError("failed to parse bank data {0} times", tries);
            }
            var delay = Random.Shared.Next(500, 2000);
            logger.LogInformation("waiting for {0} miliseconds", delay);
            await Task.Delay(delay);
        }

        return data;
    }
}