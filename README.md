# Cruise Booking API

A small .NET 8 modular monolith for listing cruises, taking bookings and sending booking confirmations. All data is held in memory, so no database, Docker or external services are needed.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- An IDE of your choice (Visual Studio, Rider or VS Code)

## Getting started

```bash
git clone https://github.com/LoveitBookit/CruiseBookingAssessment.git
cd CruiseBookingAssessment
dotnet restore
dotnet build
dotnet test
dotnet run --project src/CruiseBooking.Api --launch-profile http
```

The API runs at `http://localhost:5080`. Swagger UI is at `http://localhost:5080/swagger`.

`src/CruiseBooking.Api/CruiseBooking.Api.http` has ready-made requests for Visual Studio, Rider and the VS Code REST Client extension.

## Architecture

The application is one deployable host made of three modules. Each module owns its data and its endpoints.

| Module | Owns | Endpoints |
|---|---|---|
| **Cruises** | Cruises and their available cabins | `GET /api/cruises` |
| **Bookings** | Customer bookings | `POST /api/bookings`, `GET /api/bookings/{id}` |
| **Notifications** | Messages sent to customers | `GET /api/notifications` |

```text
src/
├── CruiseBooking.Api/                         Host: wires the modules together (Program.cs)
├── BuildingBlocks/
│   └── CruiseBooking.BuildingBlocks/          Shared plumbing: the in-process event bus
└── Modules/
    ├── Cruises/
    │   ├── CruiseBooking.Modules.Cruises/            Implementation (internal to the module)
    │   └── CruiseBooking.Modules.Cruises.Contracts/  What other modules may use
    ├── Bookings/
    │   ├── CruiseBooking.Modules.Bookings/
    │   └── CruiseBooking.Modules.Bookings.Contracts/
    └── Notifications/
        └── CruiseBooking.Modules.Notifications/
tests/
└── CruiseBooking.Tests/                       Unit, API and architecture tests
```

Each module has a `{Module}Module.cs` file that registers its services. Inside a module, code is grouped into `Api/`, `Application/`, `Domain/`, `Data/` and `Handlers/` as needed.

### How modules talk to each other

A module never references another module's implementation project. It has two ways to work with another module, and both go through that module's `.Contracts` project:

1. **Ask it something (synchronous).** The module calls an interface that the other module publishes in its Contracts. For example, the Cruises module exposes `ICruiseCatalog`, and Notifications uses it to look up a cruise's name.
2. **React to something that happened (integration event).** The module handles an event that the other module publishes. For example, Bookings publishes `BookingCreatedIntegrationEvent`, and Notifications handles it to send a confirmation.

```text
POST /api/bookings
  └─ Bookings: BookingService stores the booking
       └─ publishes BookingCreatedIntegrationEvent ──► IEventBus
                                                         └─ Notifications: SendBookingConfirmationHandler
                                                              ├─ asks Cruises via ICruiseCatalog for the cruise name
                                                              └─ stores the confirmation
```

### The event bus

`IEventBus` lives in BuildingBlocks. The implementation, `InMemoryEventBus`, delivers events in-process:

- Handlers run one after another, in the same request, before `PublishAsync` returns.
- If a handler throws, the exception goes back to the publisher.

To handle an event in a module:

1. Reference the publishing module's `.Contracts` project.
2. Implement `IIntegrationEventHandler<TEvent>`.
3. Register it in the module's `{Module}Module.cs`:

```csharp
services.AddIntegrationEventHandler<BookingCreatedIntegrationEvent, SendBookingConfirmationHandler>();
```

### Boundary rules

These are enforced by `tests/CruiseBooking.Tests/Architecture/ModuleBoundaryTests.cs`:

- A module may reference another module only through its `.Contracts` project.
- A `.Contracts` project does not reference any module implementation.
- BuildingBlocks does not reference any module.

## Sample data

The Cruises module starts with these cruises each time the application starts:

| Id | Cruise |
|---|---|
| 1 | Norwegian Pearl |
| 2 | MSC World Europa |
| 3 | Royal Caribbean |

Example booking request:

```json
{
  "cruiseId": 1,
  "customerName": "Jane Smith"
}
```

## Before the interview

Please clone the repository and make sure it builds, runs and the tests pass. Take some time to read this README and follow a booking through the code, from `POST /api/bookings` to the confirmation in `GET /api/notifications`. You don't need to change any code in advance.
