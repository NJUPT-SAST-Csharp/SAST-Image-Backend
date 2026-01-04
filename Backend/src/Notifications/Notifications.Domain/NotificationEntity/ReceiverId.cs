namespace Notifications.Domain.NotificationEntity;

public readonly record struct ReceiverId(long Value)
{
    public override string ToString()
    {
        return Value.ToString();
    }
}
