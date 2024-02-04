using Messenger;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    internal readonly struct UserCreatedMessage(long userId) : IMessage
    {
        public readonly long UserId { get; init; } = userId;
    }
}
