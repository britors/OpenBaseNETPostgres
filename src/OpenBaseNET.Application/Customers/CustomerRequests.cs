using FluentValidation;
using MediatR;
using OpenBaseNET.Domain.Entities;
using OpenBaseNET.Domain.Interfaces.Repositories;

namespace OpenBaseNET.Application.Customers;

public sealed record CustomerResponse(Guid Id, string Name);
public sealed record CreateCustomer(string Name) : IRequest<CustomerResponse>;
public sealed record GetCustomer(Guid Id) : IRequest<CustomerResponse>;
public sealed record ListCustomers(int PageNumber = 1, int PageSize = 20) : IRequest<IReadOnlyList<CustomerResponse>>;

public sealed class CreateCustomerValidator : AbstractValidator<CreateCustomer>
{
    public CreateCustomerValidator() => RuleFor(request => request.Name).NotEmpty().MaximumLength(200);
}

public sealed class ListCustomersValidator : AbstractValidator<ListCustomers>
{
    public ListCustomersValidator()
    {
        RuleFor(request => request.PageNumber).GreaterThan(0);
        RuleFor(request => request.PageSize).InclusiveBetween(1, 100);
    }
}

public sealed class CreateCustomerHandler(ICustomerRepository repository)
    : IRequestHandler<CreateCustomer, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(CreateCustomer request, CancellationToken cancellationToken)
    {
        var customer = await repository.AddAsync(new Customer { Name = request.Name.Trim() }, cancellationToken);
        return new CustomerResponse(customer.Id, customer.Name);
    }
}

public sealed class GetCustomerHandler(ICustomerRepository repository)
    : IRequestHandler<GetCustomer, CustomerResponse>
{
    public async Task<CustomerResponse> Handle(GetCustomer request, CancellationToken cancellationToken)
    {
        var customer = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException("Customer not found.");
        return new CustomerResponse(customer.Id, customer.Name);
    }
}

public sealed class ListCustomersHandler(ICustomerRepository repository)
    : IRequestHandler<ListCustomers, IReadOnlyList<CustomerResponse>>
{
    public async Task<IReadOnlyList<CustomerResponse>> Handle(ListCustomers request, CancellationToken cancellationToken)
    {
        var customers = await repository.FindAsync(cancellationToken, noTracking: true,
            pageNumber: request.PageNumber, pageSize: request.PageSize,
            orderBy: query => query.OrderBy(customer => customer.Name).ThenBy(customer => customer.Id));
        return customers.Select(customer => new CustomerResponse(customer.Id, customer.Name)).ToList();
    }
}
