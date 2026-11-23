using CruiseBooking.BuildingBlocks.Messaging;
using CruiseBooking.Modules.Bookings.Contracts;
using CruiseBooking.Modules.Bookings.Data;
using CruiseBooking.Modules.Bookings.Domain;

namespace CruiseBooking.Modules.Bookings.Application;

public class BookingService
{
    private readonly BookingStore _store;
    private readonly IEventBus _eventBus;
    private readonly TimeProvider _timeProvider;

    public BookingService(BookingStore store, IEventBus eventBus, TimeProvider timeProvider)
    {
        _store = store;
        _eventBus = eventBus;
        _timeProvider = timeProvider;
    }

    public async Task<Booking> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            CruiseId = request.CruiseId,
            CustomerName = request.CustomerName,
            CreatedAtUtc = _timeProvider.GetUtcNow().UtcDateTime,
        };

        _store.AddBooking(booking);

        await _eventBus.PublishAsync(
            new BookingCreatedIntegrationEvent(booking.Id, booking.CruiseId, booking.CustomerName, booking.CreatedAtUtc),
            cancellationToken);

        return booking;
    }

    public Booking? GetBooking(Guid id) => _store.GetBooking(id);
}
