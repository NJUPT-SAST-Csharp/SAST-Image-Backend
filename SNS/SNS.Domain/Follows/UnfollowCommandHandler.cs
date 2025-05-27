using Mediator;
using SNS.Domain.Core.Follows.Events;

namespace SNS.Domain.Follows;

internal sealed class UnfollowCommandHandler(
    IFollowManager manager,
    IDomainEventContainer container
) : ICommandHandler<UnfollowCommand>
{
    public async ValueTask<Unit> Handle(
        UnfollowCommand request,
        CancellationToken cancellationToken
    )
    {
        var follow = await manager.GetFollowAsync(
            request.Follower,
            request.Following,
            cancellationToken
        );

        if (follow is null)
        {
            return Unit.Value;
        }

        await manager.UnfollowAsync(follow, cancellationToken);

        container.AddDomainEvent(new UnfollowedDomainEvent(request.Follower, request.Following));

        return Unit.Value;
    }
}
