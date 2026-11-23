namespace CruiseBooking.Modules.Bookings.Domain;

public class Booking
{
    public Guid Id { get; init; }

    public int CruiseId { get; init; }

    public required string CustomerName { get; init; }

    public DateTime CreatedAtUtc { get; init; }
}
