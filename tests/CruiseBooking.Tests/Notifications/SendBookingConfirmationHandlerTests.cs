using CruiseBooking.Modules.Bookings.Contracts;
using CruiseBooking.Modules.Cruises.Contracts;
using CruiseBooking.Modules.Notifications.Data;
using CruiseBooking.Modules.Notifications.Handlers;
using CruiseBooking.Tests.Fakes;
using Microsoft.Extensions.Logging.Abstractions;

namespace CruiseBooking.Tests.Notifications;

public class SendBookingConfirmationHandlerTests
{
    [Fact]
    public async Task Handle_RecordsConfirmationWithCruiseName()
    {
        var store = new NotificationStore();
        var catalog = new StubCruiseCatalog(new CruiseSummary(1, "Norwegian Pearl", 5));
        var handler = new SendBookingConfirmationHandler(
            catalog, store, TimeProvider.System, NullLogger<SendBookingConfirmationHandler>.Instance);
        var bookingCreated = new BookingCreatedIntegrationEvent(Guid.NewGuid(), 1, "Jane Smith", DateTime.UtcNow);

        await handler.HandleAsync(bookingCreated, CancellationToken.None);

        var notification = Assert.Single(store.GetNotifications());
        Assert.Equal(bookingCreated.BookingId, notification.BookingId);
        Assert.Equal("Jane Smith", notification.Recipient);
        Assert.Contains("Norwegian Pearl", notification.Message);
    }
}
