using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Application.UserServices.Queries;

public sealed record class UserAvatarQuery(UserId User) : IQuery<Stream?> { }

internal sealed class UserAvatarQueryHandler(IAvatarStorageManager avatarStorageManager)
    : IQueryHandler<UserAvatarQuery, Stream?>
{
    public ValueTask<Stream?> Handle(UserAvatarQuery request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(avatarStorageManager.OpenReadStream(request.User));
    }
}
