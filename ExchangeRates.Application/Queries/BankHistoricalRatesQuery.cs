using ExchangeRates.Shared;
using FluentValidation;
using MediatR;

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
    public Task<ApplicationResult> Handle(BankHistoricalRatesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}