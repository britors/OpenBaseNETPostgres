using OpenBaseNET.Infra.Resilience.Http.Pipelines;

namespace OpenBaseNET.Infra.Resilience.Http.Extensions;

public static class HttpClientResilienceExtension
{
    public static async Task<HttpResponseMessage> PostWithRetryAsync(
        this HttpClient httpClient,
        string requestUri,
        HttpContent content,
        CancellationToken cancellationToken,
        bool allowRetry = false)
    {
        // Opt in only when the server supports an idempotency key for this operation.
        if (!allowRetry)
            return await httpClient.PostAsync(requestUri, content, cancellationToken);
        await content.LoadIntoBufferAsync(cancellationToken);
        return await HttpClientePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await httpClient.PostAsync(requestUri, content, token),
            cancellationToken);
    }

    public static async Task<HttpResponseMessage> GetWithRetryAsync(
        this HttpClient httpClient,
        string requestUri,
        CancellationToken cancellationToken)
    {
        return await HttpClientePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await httpClient.GetAsync(requestUri, token),
            cancellationToken);
    }

    public static async Task<HttpResponseMessage> PutWithRetryAsync(
        this HttpClient httpClient,
        string requestUri,
        HttpContent content,
        CancellationToken cancellationToken)
    {
        await content.LoadIntoBufferAsync(cancellationToken);
        return await HttpClientePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await httpClient.PutAsync(requestUri, content, token),
            cancellationToken);
    }

    public static async Task<HttpResponseMessage> DeleteWithRetryAsync(
        this HttpClient httpClient,
        string requestUri,
        CancellationToken cancellationToken)
    {
        return await HttpClientePipeline.AsyncRetryPipeline.ExecuteAsync(
            async token => await httpClient.DeleteAsync(requestUri, token),
            cancellationToken);
    }
}