using Notifications.Domain.NotificationEntity.NotificationEvents;
using Primitives.Entity;
using Utilities;

namespace Notifications.Domain.NotificationEntity
{
    public sealed class Notification : EntityBase<NotificationId>
    {
        private Notification()
            : base(default) { }

        private Notification(ReceiverId receiver, INotificationEvent @event)
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _receiverId = receiver;
            _message = @event.Message;
        }

        public static Notification CreateNewNotification(
            ReceiverId receiver,
            INotificationEvent @event
        )
        {
            return new(receiver, @event);
        }

        private readonly DateTime _time = DateTime.UtcNow;
        private readonly ReceiverId _receiverId;
        private readonly string _message;
    }
}
