using Messenger;

namespace SNS.WebAPI.Messages
{
    public readonly struct UserCreatedMessage : IMessage
    {
        public readonly long UserId { get; init; }
    }
}
