using Application.Shared;
using Domain.UserAggregate.UserEntity;
using Mediator;

namespace Application.UserServices.Queries;

public sealed record class UserProfileDto(
    long Id,
    string Username,
    string Nickname,
    string Biography
);

public sealed record UserProfileQuery(UserId User) : IQuery<UserProfileDto?> { }

internal sealed class UserProfileQueryHandler(
    IQueryRepository<UserProfileQuery, UserProfileDto?> repository
) : IQueryHandler<UserProfileQuery, UserProfileDto?>
{
    public async ValueTask<UserProfileDto?> Handle(
        UserProfileQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
