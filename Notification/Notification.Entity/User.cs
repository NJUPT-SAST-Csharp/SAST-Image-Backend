namespace Notification.Entity;

public sealed class User(long id, string email)
{
    public long Id { get; private init; } = id;
    public string Email { get; private init; } = email;
    public bool IsExternalNotifyEnabled { get; private set; } = false;

    private readonly List<Notification> _notifications = [];

    public void AddNotification(string content)
    {
        _notifications.Add(new Notification(content, Id));
    }

    public void ClearNotifications()
    {
        _notifications.Clear();
    }

    public IEnumerable<Notification> GetAllUnreadNotifications()
    {
        return _notifications.Where(n => n.IsRead == false);
    }
}
