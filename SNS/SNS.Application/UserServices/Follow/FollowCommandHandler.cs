using Primitives.Command;

namespace SNS.Application.UserServices.Follow
{
    internal class FollowCommandHandler : ICommandRequestHandler<FollowCommand>
    {
        public async Task Handle(FollowCommand request, CancellationToken cancellationToken) { }
    }
}
