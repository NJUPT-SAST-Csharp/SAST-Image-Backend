using Account.Domain.UserEntity.ValueObjects;
using Primitives.Exceptions;

namespace Account.Domain.UserEntity.Exceptions;

public sealed class UsernameDuplicatedDomainException(Username username) : DomainException
{
    public Username Username { get; } = username;
}
