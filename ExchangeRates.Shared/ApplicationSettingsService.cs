using Microsoft.Extensions.Options;

namespace ExchangeRates.Shared
{
    public class ApplicationSettingsService<T>
         where T : class
    {
        public T Value { get; }

        public ApplicationSettingsService(IOptions<T> options)
        {
            Value = options.Value;
        }
    }
}