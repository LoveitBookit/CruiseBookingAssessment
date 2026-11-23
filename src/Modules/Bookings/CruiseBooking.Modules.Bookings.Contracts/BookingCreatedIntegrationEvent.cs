using CruiseBooking.BuildingBlocks.Messaging;

namespace CruiseBooking.Modules.Bookings.Contracts;

public sealed record BookingCreatedIntegrationEvent(
    Guid BookingId,
    int CruiseId,
    string CustomerName,
    DateTime CreatedAtUtc) : IIntegrationEvent;
