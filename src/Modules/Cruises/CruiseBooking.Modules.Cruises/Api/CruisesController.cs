using CruiseBooking.Modules.Cruises.Data;
using CruiseBooking.Modules.Cruises.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CruiseBooking.Modules.Cruises.Api;

[ApiController]
[Route("api/cruises")]
public class CruisesController : ControllerBase
{
    private readonly CruiseStore _store;

    public CruisesController(CruiseStore store)
    {
        _store = store;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<Cruise>> GetAll() => Ok(_store.GetCruises());
}
