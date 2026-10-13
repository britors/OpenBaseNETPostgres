using FluentValidation;
using MediatR;

namespace OpenBaseNET.Application.Pipelines;

public sealed class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (!validators.Any()) return await next(cancellationToken);
        var context = new ValidationContext<TRequest>(request);
        // Validators may share a scoped DbContext, which cannot run concurrent queries.
        var validationResults = new List<FluentValidation.Results.ValidationResult>();
        foreach (var validator in validators)
            validationResults.Add(await validator.ValidateAsync(context, cancellationToken));
        var failures =
            validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
        if (failures.Count > 0)
            throw new ValidationException(failures);
        return await next(cancellationToken);
    }
}
