using CruiseBooking.BuildingBlocks.Messaging;
using CruiseBooking.Modules.Bookings.Contracts;
using CruiseBooking.Modules.Cruises.Contracts;
using CruiseBooking.Modules.Notifications.Data;
using CruiseBooking.Modules.Notifications.Domain;
using Microsoft.Extensions.Logging;

namespace CruiseBooking.Modules.Notifications.Handlers;

public class SendBookingConfirmationHandler : IIntegrationEventHandler<BookingCreatedIntegrationEvent>
{
    private readonly ICruiseCatalog _cruiseCatalog;
    private readonly NotificationStore _store;
    private readonly TimeProvider _timeProvider;
    private readonly ILogger<SendBookingConfirmationHandler> _logger;

    public SendBookingConfirmationHandler(
        ICruiseCatalog cruiseCatalog,
        NotificationStore store,
        TimeProvider timeProvider,
        ILogger<SendBookingConfirmationHandler> logger)
    {
        _cruiseCatalog = cruiseCatalog;
        _store = store;
        _timeProvider = timeProvider;
        _logger = logger;
    }

    public async Task HandleAsync(BookingCreatedIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        var cruise = await _cruiseCatalog.GetCruiseAsync(integrationEvent.CruiseId, cancellationToken);
        var cruiseName = cruise?.Name ?? $"cruise {integrationEvent.CruiseId}";

        _store.AddNotification(new Notification
        {
            Id = Guid.NewGuid(),
            BookingId = integrationEvent.BookingId,
            Recipient = integrationEvent.CustomerName,
            Message = $"Your booking on {cruiseName} is confirmed.",
            SentAtUtc = _timeProvider.GetUtcNow().UtcDateTime,
        });

        _logger.LogInformation(
            "Sent booking confirmation for booking {BookingId} to {CustomerName}",
            integrationEvent.BookingId,
            integrationEvent.CustomerName);
    }
}
