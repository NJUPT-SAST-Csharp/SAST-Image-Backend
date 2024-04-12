using Primitives.Command;
using Shared.Primitives.DomainEvent;
using SNS.Domain.Core.Follows.Events;

namespace SNS.Domain.Follows
{
    internal sealed class UnfollowCommandHandler(
        IFollowManager manager,
        IDomainEventContainer container
    ) : ICommandRequestHandler<UnfollowCommand>
    {
        private readonly IFollowManager _manager = manager;
        private readonly IDomainEventContainer _container = container;

        public async Task Handle(UnfollowCommand request, CancellationToken cancellationToken)
        {
            var follow = await _manager.GetFollowAsync(
                request.Follower,
                request.Following,
                cancellationToken
            );

            await _manager.UnfollowAsync(follow, cancellationToken);

            _container.AddDomainEvent(
                new UnfollowedDomainEvent(request.Follower, request.Following)
            );
        }
    }
}
