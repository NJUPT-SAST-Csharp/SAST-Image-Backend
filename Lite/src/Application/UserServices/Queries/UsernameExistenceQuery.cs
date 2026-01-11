using Application.Shared;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Application.UserServices.Queries;

public readonly record struct UsernameExistence(bool IsExist);

public sealed record class UsernameExistenceQuery(Username Username) : IQuery<UsernameExistence>;

internal sealed class UsernameExistenceQueryHandler(
    IQueryRepository<UsernameExistenceQuery, UsernameExistence> repository
) : IQueryHandler<UsernameExistenceQuery, UsernameExistence>
{
    public async ValueTask<UsernameExistence> Handle(
        UsernameExistenceQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
