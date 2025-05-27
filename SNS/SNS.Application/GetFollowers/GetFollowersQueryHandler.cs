using Mediator;

namespace SNS.Application.GetFollowers;

public sealed class GetFollowersQueryHandler(IFollowerRepository repository)
    : IQueryHandler<GetFollowersQuery, IEnumerable<FollowerDto>>
{
    public async ValueTask<IEnumerable<FollowerDto>> Handle(
        GetFollowersQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetFollowersAsync(request.UserId, cancellationToken);
    }
}
