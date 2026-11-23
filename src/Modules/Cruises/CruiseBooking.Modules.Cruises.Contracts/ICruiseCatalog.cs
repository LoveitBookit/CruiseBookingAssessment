namespace CruiseBooking.Modules.Cruises.Contracts;

/// <summary>
/// What the Cruises module lets other modules read about its cruises.
/// </summary>
public interface ICruiseCatalog
{
    Task<CruiseSummary?> GetCruiseAsync(int cruiseId, CancellationToken cancellationToken);
}
