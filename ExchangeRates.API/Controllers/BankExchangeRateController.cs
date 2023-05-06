using ExchangeRates.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRates.API.Controllers;

[Route("api/BankExchangeRate")]
public class BankExchangeRateController : Controller
{
    private readonly IMediator _mediator;

    public BankExchangeRateController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<BankRatesDto>> GetBankRatesAsync(BankRatesQuery query) => await _mediator.Send(query);
}