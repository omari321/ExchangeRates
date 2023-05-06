using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using ExchangeRates.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Application.Queries;

public record BankCurrencyInformationDto(string BankName, decimal BuyRate, decimal SellRate);
public record BankRatesDto(BankCurrenciesExchangeRates ExchangeRateInformation);
public record BankRatesQuery(AvailableCurrencies Currencies, DateTime? Date) : IRequest<BankRatesDto>;

public class BankRatesQueryHandler : IRequestHandler<BankRatesQuery, BankRatesDto>
{
    private readonly IRepository<BankCurrenciesExchangeRates> _repository;

    public BankRatesQueryHandler(IRepository<BankCurrenciesExchangeRates> repository)
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
        data.BankCurrencies
        return new BankRatesDto(data);
    }
}