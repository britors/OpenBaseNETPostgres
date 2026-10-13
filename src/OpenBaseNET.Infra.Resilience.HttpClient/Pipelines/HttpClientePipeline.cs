using System.Net;
using Polly;
using Polly.Retry;

namespace OpenBaseNET.Infra.Resilience.Http.Pipelines;

public static class HttpClientePipeline
{
    public static readonly ResiliencePipeline<HttpResponseMessage> AsyncRetryPipeline =
        new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>(ex => ex.StatusCode is null || IsTransient(ex.StatusCode.Value))
                    .HandleResult(response => IsTransient(response.StatusCode)),
                Delay = TimeSpan.FromSeconds(1),
                MaxRetryAttempts = 3,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    args.Outcome.Result?.Dispose();
                    return default;
                }
            }).Build();

    private static bool IsTransient(HttpStatusCode status) =>
        status is HttpStatusCode.RequestTimeout or HttpStatusCode.TooManyRequests || (int)status >= 500;
}
