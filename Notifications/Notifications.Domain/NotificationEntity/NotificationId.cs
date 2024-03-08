namespace Notifications.Domain.NotificationEntity
{
    public readonly record struct NotificationId(long Value)
    {
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
