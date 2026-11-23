using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CruiseBooking.BuildingBlocks.Messaging;

public static class MessagingServiceCollectionExtensions
{
    public static IServiceCollection AddInMemoryEventBus(this IServiceCollection services) =>
        services.AddScoped<IEventBus, InMemoryEventBus>();

    public static IServiceCollection AddIntegrationEventHandler<TEvent, THandler>(this IServiceCollection services)
        where TEvent : IIntegrationEvent
        where THandler : class, IIntegrationEventHandler<TEvent>
    {
        services.TryAddScoped<IIntegrationEventHandler<TEvent>, THandler>();

        return services;
    }
}
