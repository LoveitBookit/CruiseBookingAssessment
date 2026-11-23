using CruiseBooking.BuildingBlocks.Messaging;

namespace CruiseBooking.Tests.Fakes;

public sealed class RecordingEventBus : IEventBus
{
    public List<IIntegrationEvent> PublishedEvents { get; } = [];

    public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken)
        where TEvent : IIntegrationEvent
    {
        PublishedEvents.Add(integrationEvent);
        return Task.CompletedTask;
    }
}
