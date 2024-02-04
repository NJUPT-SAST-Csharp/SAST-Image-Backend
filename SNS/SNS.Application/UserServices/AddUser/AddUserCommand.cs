using Primitives.Command;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.AddUser
{
    public sealed class AddUserCommand(long userId) : ICommandRequest
    {
        public UserId UserId { get; } = new(userId);
    }
}
