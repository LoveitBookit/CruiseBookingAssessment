using System.ComponentModel.DataAnnotations;

namespace CruiseBooking.Modules.Bookings.Application;

public class CreateBookingRequest
{
    public int CruiseId { get; init; }

    [Required]
    public string CustomerName { get; init; } = string.Empty;
}
