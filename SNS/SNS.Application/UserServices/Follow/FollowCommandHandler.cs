using Primitives.Command;

namespace SNS.Application.UserServices.Follow
{
    internal class FollowCommandHandler : ICommandRequestHandler<FollowCommand>
    {
        public Task Handle(FollowCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
