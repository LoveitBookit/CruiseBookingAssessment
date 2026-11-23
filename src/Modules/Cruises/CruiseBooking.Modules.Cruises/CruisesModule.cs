using CruiseBooking.Modules.Cruises.Contracts;
using CruiseBooking.Modules.Cruises.Data;
using Microsoft.Extensions.DependencyInjection;

namespace CruiseBooking.Modules.Cruises;

public static class CruisesModule
{
    public static IServiceCollection AddCruisesModule(this IServiceCollection services)
    {
        services.AddScoped<CruiseStore>();
        services.AddScoped<ICruiseCatalog, CruiseCatalog>();

        return services;
    }
}
