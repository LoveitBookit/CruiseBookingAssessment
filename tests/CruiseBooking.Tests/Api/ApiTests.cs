using System.Net;
using System.Net.Http.Json;
using CruiseBooking.Modules.Bookings.Application;
using CruiseBooking.Modules.Bookings.Domain;
using CruiseBooking.Modules.Cruises.Domain;
using CruiseBooking.Modules.Notifications.Domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CruiseBooking.Tests.Api;

public class ApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCruises_ReturnsSeededCruises()
    {
        var cruises = await _client.GetFromJsonAsync<List<Cruise>>("/api/cruises");

        Assert.NotNull(cruises);
        Assert.Equal(3, cruises.Count);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task PostBooking_ReturnsCreatedBooking(int cruiseId)
    {
        var request = new CreateBookingRequest { CruiseId = cruiseId, CustomerName = "Jane Smith" };

        var response = await _client.PostAsJsonAsync("/api/bookings", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var booking = await response.Content.ReadFromJsonAsync<Booking>();
        Assert.NotNull(booking);
        Assert.Equal(cruiseId, booking.CruiseId);
    }

    [Fact]
    public async Task PostBooking_SendsConfirmationThroughNotificationsModule()
    {
        var request = new CreateBookingRequest { CruiseId = 3, CustomerName = "Jane Smith" };

        var response = await _client.PostAsJsonAsync("/api/bookings", request);
        var booking = await response.Content.ReadFromJsonAsync<Booking>();
        var notifications = await _client.GetFromJsonAsync<List<Notification>>("/api/notifications");

        Assert.NotNull(booking);
        Assert.NotNull(notifications);
        var confirmation = Assert.Single(notifications, n => n.BookingId == booking.Id);
        Assert.Contains("Royal Caribbean", confirmation.Message);
    }
}
