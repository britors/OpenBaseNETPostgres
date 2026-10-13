using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenBaseNET.Application.Customers;

namespace OpenBaseNET.Presentation.Api.Controllers;

[ApiController]
[Route("api/customers")]
public sealed class CustomersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(CreateCustomer request, CancellationToken cancellationToken)
    {
        var customer = await sender.Send(request, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> Get(Guid id, CancellationToken cancellationToken) =>
        Ok(await sender.Send(new GetCustomer(id), cancellationToken));

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<CustomerResponse>>> List(
        CancellationToken cancellationToken, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20) =>
        Ok(await sender.Send(new ListCustomers(pageNumber, pageSize), cancellationToken));
}
