using ExchangeRates.Shared;
using MediatR;

namespace ExchangeRates.Application.Queries;

public record BankHistoricalRatesQuery(DateTime StartDate, AvailableCurrencies Currencies) : IRequest<object>;


public class BankHistoricalRatesQueryHandler : IRequestHandler<BankHistoricalRatesQuery, object>
{
    public Task<object> Handle(BankHistoricalRatesQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}