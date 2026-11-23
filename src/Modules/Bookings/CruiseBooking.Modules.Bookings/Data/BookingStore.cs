using CruiseBooking.Modules.Bookings.Domain;

namespace CruiseBooking.Modules.Bookings.Data;

public class BookingStore
{
    private readonly List<Booking> _bookings = [];

    public IReadOnlyList<Booking> GetBookings() => _bookings;

    public Booking? GetBooking(Guid id) => _bookings.FirstOrDefault(b => b.Id == id);

    public void AddBooking(Booking booking) => _bookings.Add(booking);
}
