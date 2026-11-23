using CruiseBooking.Modules.Bookings.Application;
using CruiseBooking.Modules.Bookings.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CruiseBooking.Modules.Bookings.Api;

[ApiController]
[Route("api/bookings")]
public class BookingsController : ControllerBase
{
    private readonly BookingService _bookingService;

    public BookingsController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<ActionResult<Booking>> Create(CreateBookingRequest request, CancellationToken cancellationToken)
    {
        var booking = await _bookingService.CreateBookingAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }

    [HttpGet("{id:guid}")]
    public ActionResult<Booking> GetById(Guid id)
    {
        var booking = _bookingService.GetBooking(id);

        return booking is null ? NotFound() : Ok(booking);
    }
}
