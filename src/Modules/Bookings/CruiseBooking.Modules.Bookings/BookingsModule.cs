using CruiseBooking.Modules.Bookings.Application;
using CruiseBooking.Modules.Bookings.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CruiseBooking.Modules.Bookings;

public static class BookingsModule
{
    public static IServiceCollection AddBookingsModule(this IServiceCollection services)
    {
        services.AddSingleton<BookingStore>();
        services.AddScoped<BookingService>();

        return services;
    }
}
