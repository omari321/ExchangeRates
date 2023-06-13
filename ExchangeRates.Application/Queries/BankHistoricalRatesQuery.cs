using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Repositories.Abstractions;
using ExchangeRates.Shared;
using ExchangeRates.Shared.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExchangeRates.Application.Queries;

public record BankHistoricalRatesQuery(DateTime StartDate, AvailableCurrencies Currencies) : IRequest<ApplicationResult>;

public class BankHistoricalRatesQueryValidator : AbstractValidator<BankHistoricalRatesQuery>
{
    public BankHistoricalRatesQueryValidator()
    {
        RuleFor(x => x.StartDate)
            .Must(x => x.Date >= DateTime.Now.Date)
            .WithMessage(x => "start date cant be more than or equal to today");
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
        var date = request.StartDate.ToUniversalTime().Date;
        var data = await _repository
            .Query(x => x.CreateDate.Date >= date)
            .ToListAsync(cancellationToken: cancellationToken);

        var currencyInfo = data
                .Select(x => x.CurrencyRatesInformation.First(rate => rate.CurrencyName == request.Currencies.GetCurrencyNameFromEnum()));

        var bankCurrencyInfoDto = currencyInfo.Select(x => x.ExchangeRates
            .Select(rate => new BankCurrencyInformationDto(rate!.BankName, rate.BuyRate, rate.SellRate))
            .OrderByDescending(x => x.SellRate)
            .ToList());
        return new ApplicationResult
        {
            Success = true,
            Data = bankCurrencyInfoDto
        }
    }
}