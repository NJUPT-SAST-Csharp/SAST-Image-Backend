using Mediator;
using SNS.Domain.Follows.Events;

namespace SNS.Domain.Follows;

internal sealed class FollowCommandHandler(IFollowManager manager, IDomainEventContainer container)
    : ICommandHandler<FollowCommand>
{
    public async ValueTask<Unit> Handle(FollowCommand request, CancellationToken cancellationToken)
    {
        await manager.FollowAsync(request.Follower, request.Following, cancellationToken);

        container.AddDomainEvent(new FollowedDomainEvent(request.Follower, request.Following));

        return Unit.Value;
    }
}
