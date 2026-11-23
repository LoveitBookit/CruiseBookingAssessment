using CruiseBooking.Modules.Notifications.Data;
using CruiseBooking.Modules.Notifications.Domain;
using Microsoft.AspNetCore.Mvc;

namespace CruiseBooking.Modules.Notifications.Api;

[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    private readonly NotificationStore _store;

    public NotificationsController(NotificationStore store)
    {
        _store = store;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<Notification>> GetAll() => Ok(_store.GetNotifications());
}
