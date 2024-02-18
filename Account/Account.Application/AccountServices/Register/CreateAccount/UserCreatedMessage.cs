using Account.Domain.UserEntity;
using Messenger;

namespace Account.Application.Endpoints.AccountEndpoints.Register.CreateAccount
{
    internal readonly struct UserCreatedMessage(UserId userId) : IMessage
    {
        public readonly long UserId { get; init; } = userId.Value;
    }
}
