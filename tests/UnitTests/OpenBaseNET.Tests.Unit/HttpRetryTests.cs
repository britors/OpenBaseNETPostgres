using System.Net;
using OpenBaseNET.Infra.Resilience.Http.Extensions;

namespace OpenBaseNET.Tests.Unit;

public sealed class HttpRetryTests
{
    [Theory]
    [InlineData(HttpStatusCode.ServiceUnavailable)]
    [InlineData(HttpStatusCode.TooManyRequests)]
    public async Task Get_retries_transient_responses(HttpStatusCode firstStatus)
    {
        using var handler = new SequenceHandler(firstStatus);
        using var client = new HttpClient(handler);
        using var response = await client.GetWithRetryAsync("https://example.test", default);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, handler.Calls);
    }

    [Fact]
    public async Task Get_does_not_retry_client_errors()
    {
        using var handler = new SequenceHandler(HttpStatusCode.BadRequest);
        using var client = new HttpClient(handler);
        using var response = await client.GetWithRetryAsync("https://example.test", default);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(1, handler.Calls);
    }

    [Fact]
    public async Task Post_requires_explicit_opt_in_to_repeat_a_write()
    {
        using var handler = new SequenceHandler(HttpStatusCode.ServiceUnavailable);
        using var client = new HttpClient(handler);
        using var content = new StringContent("payload");
        using var response = await client.PostWithRetryAsync("https://example.test", content, default);
        Assert.Equal(HttpStatusCode.ServiceUnavailable, response.StatusCode);
        Assert.Equal(1, handler.Calls);
    }

    [Fact]
    public async Task Cancellation_is_propagated()
    {
        using var handler = new SequenceHandler(HttpStatusCode.ServiceUnavailable);
        using var client = new HttpClient(handler);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            client.GetWithRetryAsync("https://example.test", cancellation.Token));
    }

    private sealed class SequenceHandler(HttpStatusCode firstStatus) : HttpMessageHandler
    {
        public int Calls { get; private set; }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return Task.FromResult(new HttpResponseMessage(++Calls == 1 ? firstStatus : HttpStatusCode.OK));
        }
    }
}
