using CruiseBooking.BuildingBlocks.Messaging;
using CruiseBooking.Modules.Bookings;
using CruiseBooking.Modules.Cruises;
using CruiseBooking.Modules.Notifications;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddApplicationPart(typeof(CruisesModule).Assembly)
    .AddApplicationPart(typeof(BookingsModule).Assembly)
    .AddApplicationPart(typeof(NotificationsModule).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(TimeProvider.System);
builder.Services.AddInMemoryEventBus();

builder.Services
    .AddCruisesModule()
    .AddBookingsModule()
    .AddNotificationsModule();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

public partial class Program;
