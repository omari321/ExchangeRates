using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using ExchangeRates.Shared;
using ExchangeRates.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Application.Queries;

public record BankCurrencyInformationDto(string BankName, decimal BuyRate, decimal SellRate);
public record BankRatesDto(string CurrencyName, decimal OfficialRate, decimal Diff, List<BankCurrencyInformationDto>? BankCurrencyInformationDto);
public record BankRatesQuery(AvailableCurrencies Currencies, DateTime? Date) : IRequest<BankRatesDto>;

public class BankRatesQueryHandler : IRequestHandler<BankRatesQuery, BankRatesDto>
{
    private readonly IRepository<BankCurrenciesExchangeRatesEntity> _repository;

    public BankRatesQueryHandler(IRepository<BankCurrenciesExchangeRatesEntity> repository)
    {
        _repository = repository;
    }

    public async Task<BankRatesDto> Handle(BankRatesQuery request, CancellationToken cancellationToken)
    {
        var data = await _repository
            .Query(x =>
                      request.Date == null
                      ||
                      x.CreateDate.Date == request.Date!.Value.ToUniversalTime().Date)
            .FirstAsync(cancellationToken: cancellationToken);

        var currencyInfo = data.CurrencyRatesInformation
            .First(rate => rate.CurrencyName == request.Currencies.GetCurrencyNameFromEnum());

        var bankCurrencyInfoDto = currencyInfo.ExchangeRates
            .Select(rate => new BankCurrencyInformationDto(rate!.BankName, rate.BuyRate, rate.SellRate))
            .OrderByDescending(x => x.SellRate)
            .ToList();
        return new BankRatesDto(currencyInfo.CurrencyName, currencyInfo.OfficialRate, currencyInfo.Diff, bankCurrencyInfoDto);
    }
}