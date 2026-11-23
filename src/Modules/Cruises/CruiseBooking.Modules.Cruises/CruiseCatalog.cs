using CruiseBooking.Modules.Cruises.Contracts;
using CruiseBooking.Modules.Cruises.Data;

namespace CruiseBooking.Modules.Cruises;

public class CruiseCatalog : ICruiseCatalog
{
    private readonly CruiseStore _store;

    public CruiseCatalog(CruiseStore store)
    {
        _store = store;
    }

    public Task<CruiseSummary?> GetCruiseAsync(int cruiseId, CancellationToken cancellationToken)
    {
        var cruise = _store.GetCruise(cruiseId);

        var summary = cruise is null
            ? null
            : new CruiseSummary(cruise.Id, cruise.Name, cruise.AvailableCabins);

        return Task.FromResult(summary);
    }
}
