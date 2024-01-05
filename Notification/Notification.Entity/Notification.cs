using Shared.Utilities;

namespace Notification.Entity
{
    public sealed class Notification
    {
        private Notification() { }

        internal Notification(string content, long receiverId)
        {
            Content = content;
            ReceiverId = receiverId;
        }

        public long Id { get; private init; } = SnowFlakeIdGenerator.NewId;

        public string Content { get; private init; }

        public bool IsRead { get; private set; } = false;

        public bool IsSent { get; private set; } = false;

        public long ReceiverId { get; private init; }

        public DateTime Time { get; private init; } = DateTime.Now;
    }
}
