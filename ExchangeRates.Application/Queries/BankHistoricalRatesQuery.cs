using ExchangeRates.Application.Queries.Dtos;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using ExchangeRates.Shared;
using ExchangeRates.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Application.Queries;

public record IntermediaryTableForHistoricExchanges(DateOnly Date, ExchangeRateEntity ExchangeRateEntity);
public record BankHistoricalRatesQuery(int DaysCount, AvailableCurrencies Currencies) : IRequest<ApplicationResult>;

public class BankHistoricalRatesQueryValidator : AbstractValidator<BankHistoricalRatesQuery>
{
    public BankHistoricalRatesQueryValidator()
    {
        RuleFor(x => x.DaysCount)
            .NotNull()
            .NotEmpty();
        RuleFor(x => x.Currencies)
            .NotNull()
            .IsInEnum();
    }
}

public class BankHistoricalRatesQueryHandler : IRequestHandler<BankHistoricalRatesQuery, ApplicationResult>
{
    private readonly IRepository<BankCurrenciesExchangeRatesEntity> _repository;

    public BankHistoricalRatesQueryHandler(IRepository<BankCurrenciesExchangeRatesEntity> repository)
    {
        _repository = repository;
    }

    public async Task<ApplicationResult> Handle(BankHistoricalRatesQuery request, CancellationToken cancellationToken)
    {
        var date = DateTime.Now.ToUniversalTime().Date.AddDays(-request.DaysCount);
        var data = await _repository
            .Query(x => x.CreateDate >= date)
            .ToListAsync(cancellationToken: cancellationToken);



        var currencyInfo = data
                .Select(x =>
                    new IntermediaryTableForHistoricExchanges(DateOnly.FromDateTime(x.CreateDate.LocalDateTime),
                        x.CurrencyRatesInformation.First(rate => rate.CurrencyName == request.Currencies.GetCurrencyNameFromEnum()))).ToList();

        var bankCurrencyInfoDto = currencyInfo.Select(x =>
            new BankHistoricalRatesDto
            {
                Date = x.Date,
                BankRates = x.ExchangeRateEntity.ExchangeRates
                    .Select(y => new BankRates(y!.BankName, y.BuyRate, y.SellRate)).ToList()
            });
        return new ApplicationResult
        {
            Success = true,
            Data = bankCurrencyInfoDto
        };
    }
}