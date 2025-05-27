using Mediator;

namespace SNS.Application.GetFollowCount;

public sealed class GetFollowCountQueryHandler(IFollowCountRepository repository)
    : IQueryHandler<GetFollowCountQuery, FollowCountDto>
{
    public async ValueTask<FollowCountDto> Handle(
        GetFollowCountQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetFollowCountAsync(request.UserId, cancellationToken);
    }
}
