using CruiseBooking.Modules.Bookings.Application;
using CruiseBooking.Modules.Bookings.Contracts;
using CruiseBooking.Modules.Bookings.Data;
using CruiseBooking.Tests.Fakes;

namespace CruiseBooking.Tests.Bookings;

public class BookingServiceTests
{
    private readonly BookingStore _store = new();
    private readonly RecordingEventBus _eventBus = new();
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _service = new BookingService(_store, _eventBus, TimeProvider.System);
    }

    [Fact]
    public async Task CreateBooking_StoresBookingForRequestedCruise()
    {
        var request = new CreateBookingRequest { CruiseId = 1, CustomerName = "Jane Smith" };

        var booking = await _service.CreateBookingAsync(request, CancellationToken.None);

        Assert.Equal(1, booking.CruiseId);
        Assert.Equal("Jane Smith", booking.CustomerName);
        Assert.Same(booking, _store.GetBooking(booking.Id));
    }

    [Fact]
    public async Task CreateBooking_PublishesBookingCreatedEvent()
    {
        var request = new CreateBookingRequest { CruiseId = 1, CustomerName = "Jane Smith" };

        var booking = await _service.CreateBookingAsync(request, CancellationToken.None);

        var published = Assert.IsType<BookingCreatedIntegrationEvent>(Assert.Single(_eventBus.PublishedEvents));
        Assert.Equal(booking.Id, published.BookingId);
        Assert.Equal(1, published.CruiseId);
    }
}
