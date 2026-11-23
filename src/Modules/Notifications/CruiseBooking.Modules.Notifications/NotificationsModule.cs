using CruiseBooking.BuildingBlocks.Messaging;
using CruiseBooking.Modules.Bookings.Contracts;
using CruiseBooking.Modules.Notifications.Data;
using CruiseBooking.Modules.Notifications.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CruiseBooking.Modules.Notifications;

public static class NotificationsModule
{
    public static IServiceCollection AddNotificationsModule(this IServiceCollection services)
    {
        services.AddSingleton<NotificationStore>();
        services.AddIntegrationEventHandler<BookingCreatedIntegrationEvent, SendBookingConfirmationHandler>();

        return services;
    }
}
