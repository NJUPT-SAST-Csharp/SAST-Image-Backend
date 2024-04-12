using Primitives.Command;
using Shared.Primitives.DomainEvent;
using SNS.Domain.Follows.Events;

namespace SNS.Domain.Follows
{
    internal sealed class FollowCommandHandler(
        IFollowManager manager,
        IDomainEventContainer container
    ) : ICommandRequestHandler<FollowCommand>
    {
        private readonly IFollowManager _manager = manager;
        private readonly IDomainEventContainer _container = container;

        public async Task Handle(FollowCommand request, CancellationToken cancellationToken)
        {
            await _manager.FollowAsync(request.Follower, request.Following, cancellationToken);

            _container.AddDomainEvent(new FollowedDomainEvent(request.Follower, request.Following));
        }
    }
}
