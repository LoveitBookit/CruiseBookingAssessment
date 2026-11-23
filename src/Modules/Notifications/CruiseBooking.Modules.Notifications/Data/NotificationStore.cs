using CruiseBooking.Modules.Notifications.Domain;

namespace CruiseBooking.Modules.Notifications.Data;

public class NotificationStore
{
    private readonly List<Notification> _notifications = [];

    public IReadOnlyList<Notification> GetNotifications() => _notifications;

    public void AddNotification(Notification notification) => _notifications.Add(notification);
}
