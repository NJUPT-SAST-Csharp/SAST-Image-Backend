using Domain.Extensions;
using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Exceptions;

public sealed class UsernameDuplicateException(Username username) : DomainException
{
    public Username Username { get; } = username;
}
