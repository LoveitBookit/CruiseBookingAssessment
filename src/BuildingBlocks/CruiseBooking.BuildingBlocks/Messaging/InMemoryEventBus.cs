using Microsoft.Extensions.DependencyInjection;

namespace CruiseBooking.BuildingBlocks.Messaging;

/// <summary>
/// Delivers events in-process: every registered handler runs, one after another, before
/// <see cref="PublishAsync{TEvent}"/> returns. An exception from a handler propagates to the publisher.
/// </summary>
public sealed class InMemoryEventBus : IEventBus
{
    private readonly IServiceProvider _serviceProvider;

    public InMemoryEventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken)
        where TEvent : IIntegrationEvent
    {
        var handlers = _serviceProvider.GetServices<IIntegrationEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            await handler.HandleAsync(integrationEvent, cancellationToken);
        }
    }
}
