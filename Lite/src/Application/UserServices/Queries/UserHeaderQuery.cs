using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Application.UserServices.Queries;

public sealed record UserHeaderQuery(UserId User) : IQuery<Stream?> { }

internal sealed class UserHeaderQueryHandler(IHeaderStorageManager manager)
    : IQueryHandler<UserHeaderQuery, Stream?>
{
    public ValueTask<Stream?> Handle(UserHeaderQuery request, CancellationToken cancellationToken)
    {
        return ValueTask.FromResult(manager.OpenReadStream(request.User));
    }
}
