namespace Notifications.Domain.NotificationEntity.NotificationEvents;

public interface INotificationEvent
{
    public string Message { get; }
}
