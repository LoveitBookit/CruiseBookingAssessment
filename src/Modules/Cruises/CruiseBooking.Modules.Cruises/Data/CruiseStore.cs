using CruiseBooking.Modules.Cruises.Domain;

namespace CruiseBooking.Modules.Cruises.Data;

public class CruiseStore
{
    private readonly List<Cruise> _cruises =
    [
        new Cruise { Id = 1, Name = "Norwegian Pearl", AvailableCabins = 5 },
        new Cruise { Id = 2, Name = "MSC World Europa", AvailableCabins = 0 },
        new Cruise { Id = 3, Name = "Royal Caribbean", AvailableCabins = 10 },
    ];

    public IReadOnlyList<Cruise> GetCruises() => _cruises;

    public Cruise? GetCruise(int id) => _cruises.FirstOrDefault(c => c.Id == id);
}
