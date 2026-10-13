using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenBaseNET.Infra.Mediator;

namespace OpenBaseNET.Tests.Unit;

public sealed class MediatorTests
{
    [Fact]
    public async Task Void_requests_and_notifications_are_registered_without_generic_requests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddSingleton<Counter>();
        services.AddMediatorApi(new ConfigurationBuilder().Build(), typeof(MediatorTests).Assembly);
        await using var provider = services.BuildServiceProvider();
        var mediator = provider.GetRequiredService<IMediator>();
        await mediator.Send(new VoidRequest());
        await mediator.Publish(new Notification());
        Assert.Equal(2, provider.GetRequiredService<Counter>().Value);
    }

    public sealed record VoidRequest : IRequest;
    public sealed record Notification : INotification;
    public sealed class Counter { public int Value { get; set; } }
    public sealed class VoidHandler(Counter counter) : IRequestHandler<VoidRequest>
    {
        public Task Handle(VoidRequest request, CancellationToken token) { counter.Value++; return Task.CompletedTask; }
    }
    public sealed class NotificationHandler(Counter counter) : INotificationHandler<Notification>
    {
        public Task Handle(Notification notification, CancellationToken token) { counter.Value++; return Task.CompletedTask; }
    }
}
