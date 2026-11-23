namespace CruiseBooking.Modules.Notifications.Domain;

public class Notification
{
    public Guid Id { get; init; }

    public Guid BookingId { get; init; }

    public required string Recipient { get; init; }

    public required string Message { get; init; }

    public DateTime SentAtUtc { get; init; }
}
