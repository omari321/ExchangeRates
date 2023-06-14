using ExchangeRates.Application.Queries.Dtos;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using ExchangeRates.Shared;
using ExchangeRates.Shared.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Application.Queries;

public record BankRatesDto(string CurrencyName, decimal OfficialRate, decimal Diff, List<BankCurrencyInformationDto>? BankCurrencyInformationDto);
public record BankRatesQuery(AvailableCurrencies Currencies, DateTime? Date) : IRequest<ApplicationResult<BankRatesDto>>;

public class BankRatesQueryHandler : IRequestHandler<BankRatesQuery, ApplicationResult<BankRatesDto>>
{
    private readonly IRepository<BankCurrenciesExchangeRatesEntity> _repository;

    public BankRatesQueryHandler(IRepository<BankCurrenciesExchangeRatesEntity> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult<BankRatesDto>> Handle(BankRatesQuery request, CancellationToken cancellationToken)
    {
        var date = request.Date?.ToUniversalTime().Date ?? DateTime.Now.ToUniversalTime().Date;
        var data = await _repository
            .Query(x => x.CreateDate.Date == date)
            .FirstAsync(cancellationToken: cancellationToken);

        var currencyInfo = data.CurrencyRatesInformation
            .First(rate => rate.CurrencyName == request.Currencies.GetCurrencyNameFromEnum());

        var bankCurrencyInfoDto = currencyInfo.ExchangeRates
            .Select(rate => new BankCurrencyInformationDto(rate!.BankName, rate.BuyRate, rate.SellRate))
            .OrderByDescending(x => x.SellRate)
            .ToList();

        return new ApplicationResult<BankRatesDto>
        {
            Success = true,
            Data = new BankRatesDto(currencyInfo.CurrencyName, currencyInfo.OfficialRate, currencyInfo.Diff,
                bankCurrencyInfoDto)
        };

    }
}