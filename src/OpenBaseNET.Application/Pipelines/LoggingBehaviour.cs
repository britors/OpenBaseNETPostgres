using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace OpenBaseNET.Application.Pipelines;

public sealed class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        try
        {

            logger.LogInformation(
                    "Chamando o comando {RequestName} com valores {Values}",
                    requestName,
                    JsonSerializer.Serialize(request)
                );

            var response = await next();

            logger.LogInformation(
                    "Resposta para  o comando {RequestName} foi {Values}",
                    requestName,
                    JsonSerializer.Serialize(response)
                );

            return response;
        }
        catch (Exception exception)
        {
            logger.LogError(
                    exception,
                    "Ocorreu um erro ao chamar o comando{RequestName}",
                    requestName
                );

            return await next();
        }
    }
}