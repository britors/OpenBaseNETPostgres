using MediatR;

using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace OpenBaseNET.Application.Pipelines;

public sealed class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var started = Stopwatch.GetTimestamp();
        try
        {
            return await next(cancellationToken);
        }
        finally
        {
            logger.LogInformation("Handled {RequestType} in {ElapsedMilliseconds} ms",
                typeof(TRequest).Name, Stopwatch.GetElapsedTime(started).TotalMilliseconds);
        }
    }
}
