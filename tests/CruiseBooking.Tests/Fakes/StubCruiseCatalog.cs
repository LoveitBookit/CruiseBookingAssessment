using CruiseBooking.Modules.Cruises.Contracts;

namespace CruiseBooking.Tests.Fakes;

public sealed class StubCruiseCatalog : ICruiseCatalog
{
    private readonly Dictionary<int, CruiseSummary> _cruises;

    public StubCruiseCatalog(params CruiseSummary[] cruises)
    {
        _cruises = cruises.ToDictionary(c => c.Id);
    }

    public Task<CruiseSummary?> GetCruiseAsync(int cruiseId, CancellationToken cancellationToken) =>
        Task.FromResult(_cruises.GetValueOrDefault(cruiseId));
}
