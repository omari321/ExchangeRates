using ExchangeRates.Shared;
using MediatR;

namespace ExchangeRates.Application.Queries;

public record BankHistoricalRatesQuery(DateTime StartDate, AvailableCurrencies Currencies) : IRequest<ApplicationResult>;



public class BankHistoricalRatesQueryHandler : IRequestHandler<BankHistoricalRatesQuery, ApplicationResult>
{
    public Task<ApplicationResult> Handle(BankHistoricalRatesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}