using Mediator;

namespace SNS.Application.GetFollowing;

public sealed class GetFollowingQueryHandler(IFollowingRepository repository)
    : IQueryHandler<GetFollowingQuery, IEnumerable<FollowingDto>>
{
    public async ValueTask<IEnumerable<FollowingDto>> Handle(
        GetFollowingQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetFollowingAsync(request.UserId, cancellationToken);
    }
}
