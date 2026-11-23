namespace CruiseBooking.Modules.Cruises.Domain;

public class Cruise
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public int AvailableCabins { get; set; }
}
